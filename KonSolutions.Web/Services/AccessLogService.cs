using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.Extensions.Configuration;

namespace KONSolutions.Web.Services;

/// <summary>
/// Log de accesos y eventos del usuario. Registra:
/// página/módulo visitado, evento disparado, y los parámetros usados.
/// </summary>
public class AccessLogService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;
    // FIX SEGURIDAD (Vuln 10): Console.WriteLine controlado por appsettings.json.
    // En producción poner "Logging:ConsolaNavegador": false para no exponer
    // parámetros de negocio en DevTools del navegador.
    private readonly bool _consolaActiva;
    private readonly bool _persistirEnApi;

    public AccessLogService(HttpClient http, ILocalStorageService localStorage, IConfiguration config)
    {
        _http           = http;
        _localStorage   = localStorage;
        _consolaActiva  = config.GetValue<bool>("Logging:ConsolaNavegador", false);
        _persistirEnApi = config.GetValue<bool>("Logging:PersistirEnApi",   false);
    }

    /// <summary>Registra la navegación a una página o módulo.</summary>
    public Task LogNavegacionAsync(string modulo, string pagina)
        => RegistrarAsync(modulo, pagina, "NAVEGACION", null);

    /// <summary>Registra un evento disparado por el usuario (con sus parámetros).</summary>
    public Task LogEventoAsync(string modulo, string pagina, string evento, object? parametros = null)
        => RegistrarAsync(modulo, pagina, evento,
               parametros is null ? null : System.Text.Json.JsonSerializer.Serialize(parametros));

    private async Task RegistrarAsync(string modulo, string pagina, string evento, string? parametros)
    {
        var codUsuario = "ANONIMO";
        try
        {
            var sesion = await _localStorage.GetItemAsync<KONSolutions.Shared.Models.Auth.UsuarioSesion>("usuarioSesion");
            if (sesion is not null) codUsuario = sesion.COD_USUARIO;
        }
        catch { }

        var entry = new
        {
            COD_USUARIO = codUsuario,
            Modulo      = modulo,
            Pagina      = pagina,
            Evento      = evento,
            Parametros  = parametros ?? string.Empty,
            Fecha       = DateTime.Now
        };

        // Solo escribe en consola si está habilitado en appsettings.json
        // ("Logging:ConsolaNavegador": true). Desactivado por defecto en producción.
        if (_consolaActiva)
            Console.WriteLine($"[ACCESO] {codUsuario} · {modulo}/{pagina} · {evento}" +
                              (parametros is null ? "" : $" · params: {parametros}"));

        // Persistir en API (no rompe si falla).
        if (_persistirEnApi)
        {
            try { await _http.PostAsJsonAsync(ApiRoutes.Developer.AccessLog, entry); }
            catch { }
        }
    }
}
