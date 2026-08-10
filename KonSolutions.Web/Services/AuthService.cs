using System.Linq;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.JSInterop;
using KONSolutions.Shared.Common;
using KONSolutions.Shared.Models.Auth;

namespace KONSolutions.Web.Services;

public interface IAuthService
{
    Task<(bool ok, string? error)> LoginAsync(LoginRequest request);
    /// <param name="conservarCacheMenu">true solo para el logout de arranque de MainLayout
    /// — ver el comentario completo en la implementación.</param>
    Task LogoutAsync(bool conservarCacheMenu = false);
    Task<UsuarioSesion?> GetSesionAsync();
    Task<(bool ok, string? error)> ValidarPasswordAsync(string codUsuario, string password);
    /// <summary>Logo de la entidad de dependencia — solo en memoria (no persiste en
    /// localStorage, ver comentario en LoginAsync). Válido mientras dure la pestaña; tras
    /// un F5 vuelve a null hasta el próximo login.</summary>
    byte[]? LogoDependenciaEnMemoria { get; }
}

public class AuthService : IAuthService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;
    private readonly JwtAuthStateProvider _authProvider;
    private readonly AccessLogService _accessLog;
    private readonly IJSRuntime _js;

    public byte[]? LogoDependenciaEnMemoria { get; private set; }

    public AuthService(HttpClient http, ILocalStorageService localStorage,
        JwtAuthStateProvider authProvider, AccessLogService accessLog, IJSRuntime js)
    {
        _http = http; _localStorage = localStorage;
        _authProvider = authProvider; _accessLog = accessLog; _js = js;
    }

    public async Task<(bool ok, string? error)> LoginAsync(LoginRequest request)
    {
        try
        {
            var resp = await _http.PostAsJsonAsync(ApiRoutes.Auth.Login, request);
            var raw = await resp.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(raw))
                return (false, resp.IsSuccessStatusCode
                    ? "El servidor no devolvió datos de sesión."
                    : $"Error de autenticación (HTTP {(int)resp.StatusCode}).");

            ApiResponse<LoginResponse>? result;
            try
            {
                result = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<LoginResponse>>(raw,
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (System.Text.Json.JsonException)
            {
                return (false, "La respuesta del servidor no tiene el formato esperado.");
            }

            if (!resp.IsSuccessStatusCode || result is null || !result.Success || result.Data is null)
            {
                if (result?.Message == "DEBE_DEFINIR_PASSWORD")
                    return (false, "Tu cuenta aún no tiene contraseña definida. Contacta al administrador del sistema.");
                return (false, result?.Message ?? "Credenciales inválidas.");
            }

            // IMG_LOGO_DEPENDENCIA puede pesar varios cientos de KB/pocos MB — persistir el
            // objeto Usuario completo (con la imagen en Base64 adentro) en localStorage
            // superaba la cuota del navegador (~5-10MB) y tiraba QuotaExceededError,
            // rompiendo el login. Se guarda solo en memoria (dura lo que dure la pestaña,
            // se resetea con un F5) y se quita del objeto antes de persistirlo.
            LogoDependenciaEnMemoria = result.Data.Usuario?.IMG_LOGO_DEPENDENCIA;
            if (result.Data.Usuario is not null) result.Data.Usuario.IMG_LOGO_DEPENDENCIA = null;

            await _localStorage.SetItemAsync("authToken", result.Data.Token);
            await _localStorage.SetItemAsync("usuarioSesion", result.Data.Usuario);
            _authProvider.NotifyUserAuthentication(result.Data.Token);
            // Deja el token en memoria del handler: así ninguna petición depende de volver a
            // leer localStorage (una llamada de JS interop por request, que con los sondeos
            // en paralelo podía fallar y hacer que la petición saliera sin autenticar).
            // También reinicia la bandera de "sesión expirada ya avisada": sin eso, una vez
            // disparada quedaba en true para siempre y la app no volvía a avisar en la
            // siguiente expiración, quedándose con los sondeos en 401 silencioso.
            JwtMessageHandler.SetToken(result.Data.Token);

            await _accessLog.LogEventoAsync("AUTH", "Login", "LOGIN_OK",
                new { request.COD_USUARIO });

            // Si el usuario debe cambiar su password (NULL o vencido), avisar al front.
            if (result.Data.Usuario?.FLG_CHANGE_PASSSWORD == true)
                return (true, "CAMBIO_PASSWORD");

            return (true, null);
        }
        catch (HttpRequestException)
        {
            return (false, "No se pudo conectar con el servidor.");
        }
        catch (Exception ex)
        {
            return (false, $"Error inesperado: {ex.Message}");
        }
    }

    /// <param name="conservarCacheMenu">
    /// true = no borra la caché del árbol de menú (botonesTreeview_*).
    ///
    /// Lo usa el logout de ARRANQUE de MainLayout, que corre en cada primera carga de la
    /// aplicación para forzar el login. Ese logout borraba la caché justo antes de cada
    /// login, así que el mecanismo stale-while-revalidate de MenuService (que pinta el
    /// menú al instante desde localStorage y refresca en segundo plano) NUNCA llegaba a
    /// usarse en el camino de login: todos los logins pagaban la carga completa y
    /// bloqueante. Medido: 33,4 s de splash.
    ///
    /// Se puede conservar sin riesgo de mezclar datos entre usuarios porque la caché está
    /// segmentada por rol (botonesTreeview_{rol}), tiene TTL de 60 min, y el logout REAL
    /// (el del botón "Cerrar sesión", que llama sin este parámetro) la sigue limpiando —
    /// igual que CambiarRolAsync y ForzarRecargaAsync.
    /// </param>
    public async Task LogoutAsync(bool conservarCacheMenu = false)
    {
        await _accessLog.LogEventoAsync("AUTH", "Logout", "LOGOUT", null);
        JwtMessageHandler.ClearToken();   // que no quede el token cacheado en memoria
        LogoDependenciaEnMemoria = null;
        await _localStorage.RemoveItemAsync("authToken");
        await _localStorage.RemoveItemAsync("usuarioSesion");
        await _localStorage.RemoveItemAsync("sesionBloqueada");   // limpiar bloqueo al salir

        // Limpiar TODA la caché del menú (botonesTreeview_*) — si no se limpia aquí,
        // el árbol de menú queda con datos viejos hasta que expire el TTL de 60 min,
        // aunque el usuario cierre sesión y vuelva a entrar (o entre otro usuario/rol).
        if (!conservarCacheMenu)
        {
            try
            {
                var claves = await _localStorage.KeysAsync();
                foreach (var k in claves.Where(k => k.StartsWith("botonesTreeview_")).ToList())
                    await _localStorage.RemoveItemAsync(k);
            }
            catch { }
        }

        _authProvider.NotifyUserLogout();
    }

    public async Task<UsuarioSesion?> GetSesionAsync()
        => await _localStorage.GetItemAsync<UsuarioSesion>("usuarioSesion");

    /// <summary>
    /// Valida usuario+contraseña contra el SP de login SIN tocar la sesión actual.
    /// Se usa para desbloquear la pantalla de bloqueo por inactividad (punto 15).
    /// </summary>
    public async Task<(bool ok, string? error)> ValidarPasswordAsync(string codUsuario, string password)
    {
        try
        {
            var resp = await _http.PostAsJsonAsync(ApiRoutes.Auth.Login,
                new LoginRequest { COD_USUARIO = codUsuario, COD_PASSWORD = password });
            var raw = await resp.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(raw)) return (false, "Sin respuesta.");

            var result = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<LoginResponse>>(raw,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Válido si el login fue exitoso y devolvió token (la contraseña es correcta).
            // No dependemos de FLG_ACCESO porque puede no venir poblado en esta respuesta.
            if (resp.IsSuccessStatusCode && result?.Success == true
                && !string.IsNullOrWhiteSpace(result.Data?.Token))
                return (true, null);
            return (false, result?.Message ?? "Contraseña incorrecta.");
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }
}
