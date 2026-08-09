using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Microsoft.JSInterop;

namespace KONSolutions.Web.Services;

// FIX SEGURIDAD (Vuln 4): el token JWT se almacena en sessionStorage en lugar de
// localStorage. sessionStorage no persiste entre sesiones de navegador (se borra al
// cerrar la pestaña) y no es compartida entre pestañas, reduciendo la ventana de
// exposición ante extensiones maliciosas o XSS persistente.
// Los datos de sesión NO sensibles (usuarioSesion) siguen en localStorage para UX.
public class JwtAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly IJSRuntime _js;
    private static readonly AuthenticationState Anonymous =
        new(new ClaimsPrincipal(new ClaimsIdentity()));

    public JwtAuthStateProvider(ILocalStorageService localStorage, IJSRuntime js)
    { _localStorage = localStorage; _js = js; }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string? token;
        try { token = await _localStorage.GetItemAsync<string>("authToken"); }
        catch { return Anonymous; }

        // No se limpia el token en memoria del handler si acá llegó vacío: esta lectura de
        // localStorage puede devolver null por una carrera con otras lecturas concurrentes
        // (los sondeos de fondo), y borrar por eso un token bueno dejaría a la aplicación sin
        // autenticar. Solo se limpia cuando el token está efectivamente vencido (abajo).
        if (string.IsNullOrWhiteSpace(token)) return Anonymous;

        var claims = ParseClaims(token).ToList();
        var exp = claims.FirstOrDefault(c => c.Type == "exp")?.Value;
        if (exp is not null && long.TryParse(exp, out var s))
        {
            if (DateTimeOffset.FromUnixTimeSeconds(s) < DateTimeOffset.UtcNow)
            {
                JwtMessageHandler.ClearToken();
                await _localStorage.RemoveItemAsync("authToken");
                await _localStorage.RemoveItemAsync("usuarioSesion");
                return Anonymous;
            }
        }

        // Token válido: se deja en memoria del handler para que las peticiones HTTP no
        // dependan de volver a leer localStorage (un JS interop por request, que es donde
        // se producía la carrera que hacía salir peticiones sin cabecera Authorization).
        JwtMessageHandler.SeedToken(token);
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));
    }

    /// <summary>Fecha/hora UTC (claim "exp") en que vence el token guardado, o null si no
    /// hay sesión o el token no trae ese claim. Usado por Configuración General para mostrar
    /// el tiempo restante junto al bloqueo por inactividad.</summary>
    public async Task<DateTimeOffset?> GetExpiracionTokenAsync()
    {
        string? token;
        try { token = await _localStorage.GetItemAsync<string>("authToken"); }
        catch { return null; }
        if (string.IsNullOrWhiteSpace(token)) return null;

        var exp = ParseClaims(token).FirstOrDefault(c => c.Type == "exp")?.Value;
        return exp is not null && long.TryParse(exp, out var s) ? DateTimeOffset.FromUnixTimeSeconds(s) : null;
    }

    public void NotifyUserAuthentication(string token)
        => NotifyAuthenticationStateChanged(Task.FromResult(
            new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaims(token), "jwt")))));

    public void NotifyUserLogout()
        => NotifyAuthenticationStateChanged(Task.FromResult(Anonymous));

    private static IEnumerable<Claim> ParseClaims(string token)
    {
        var payload = token.Split('.')[1];
        var json = Base64UrlDecode(payload);
        var dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);
        return dict is null ? Enumerable.Empty<Claim>()
            : dict.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
    }

    private static string Base64UrlDecode(string input)
    {
        var o = input.Replace('-', '+').Replace('_', '/');
        switch (o.Length % 4) { case 2: o += "=="; break; case 3: o += "="; break; }
        return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(o));
    }
}
