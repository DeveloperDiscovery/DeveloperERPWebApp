using System.Net;
using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KONSolutions.Web.Services;

/// <summary>
/// Añade el token JWT a cada request saliente y muestra el spinner de cursor
/// (estilo iOS, transparente) mientras dura cualquier carga o guardado.
/// También detecta globalmente cuando el token expiró (401) — sin esto, cada
/// pantalla individual truena tratando de leer JSON de una respuesta 401 vacía
/// (los 401 no traen cuerpo), con un error críptico de parseo en vez de avisarle
/// al usuario que su sesión expiró.
/// </summary>
public class JwtMessageHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;
    private readonly IJSRuntime _js;
    private readonly JwtAuthStateProvider _authProvider;
    private readonly NavigationManager _nav;
    // Antes era un bool "ya se avisó" que se ponía en true una sola vez para toda la sesión.
    // Si esa ÚNICA redirección no llegaba a completarse de verdad (NavigateTo no lanza
    // excepción aunque el navegador por algún motivo no navegue), la bandera quedaba trabada
    // en true para siempre y ningún 401 futuro (de los sondeos que siguen corriendo cada
    // 10-20s) volvía a intentarlo — la app quedaba mostrando 401 en consola sin fin, sin
    // mandar nunca al login. Con un cooldown por tiempo, cada ráfaga de 401 simultáneos se
    // sigue deduplicando, pero el SIGUIENTE sondeo (unos segundos después) vuelve a intentar
    // la redirección si la anterior no sacó al usuario de la página.
    private static DateTime _ultimoIntentoRedirect = DateTime.MinValue;

    /// <summary>
    /// Token en memoria. Antes se leía localStorage en CADA petición, y eso implica una
    /// llamada de JS interop por request: con los sondeos en paralelo (notificaciones cada
    /// 20s, control de cambios cada 10s, health cada 2 min — todos con System.Threading.Timer)
    /// esas lecturas concurrentes pueden fallar, y como el error se ignoraba, la petición
    /// salía SIN cabecera Authorization y el servidor respondía 401. Teniéndolo en memoria se
    /// elimina esa condición de carrera y además se ahorra un interop por request.
    /// localStorage sigue siendo la fuente de verdad al recargar la página (F5).
    /// </summary>
    private static string? _tokenEnMemoria;

    /// <summary>La establece AuthService tras un login exitoso.</summary>
    public static void SetToken(string token)
    {
        _tokenEnMemoria = token;
        _ultimoIntentoRedirect = DateTime.MinValue;
    }

    /// <summary>La limpia AuthService al cerrar sesión.</summary>
    public static void ClearToken() => _tokenEnMemoria = null;

    /// <summary>
    /// La llama JwtAuthStateProvider cuando lee un token válido de localStorage. Ese provider
    /// ya hace esa lectura al arrancar la aplicación (y en cada verificación de autorización),
    /// así que aprovecharla deja el token disponible en memoria antes de la primera petición
    /// HTTP: sin esto, tras un F5 la primera petición todavía tenía que leer localStorage y
    /// podía perder la carrera contra los sondeos, saliendo sin cabecera Authorization.
    /// A diferencia de SetToken, no toca la bandera de "sesión expirada ya avisada".
    /// </summary>
    public static void SeedToken(string token)
    {
        if (!string.IsNullOrWhiteSpace(token)) _tokenEnMemoria = token;
    }

    /// <summary>
    /// MainLayout la pone en true mientras dura el splash inicial (configuración + menú)
    /// y en false apenas termina. Con esto en true, este handler NO muestra su propio
    /// spinner global por cada request — evita que compita visualmente con el círculo
    /// de porcentaje del splash, que ya es el único indicador de carga que debe verse
    /// en ese momento.
    /// </summary>
    public static bool SplashActivo { get; set; }

    /// <summary>
    /// Clave de HttpRequestOptions para marcar requests silenciosos (sondeos en segundo
    /// plano como el control de cambios) que NO deben disparar el spinner global — evita
    /// el parpadeo del anillo+logo cada vez que el timer de sondeo dispara su GET.
    /// </summary>
    public static readonly HttpRequestOptionsKey<bool> SinSpinnerKey = new("SinSpinner");

    /// <summary>
    /// ControlCambiosClientService la pone en true mientras ejecuta los callbacks de
    /// refresco de las pantallas suscritas (cuando el sondeo detecta un cambio real) —
    /// esos Load() hacen sus propios requests normales, y sin este flag encenderían el
    /// spinner global igual. Con esto, el proceso de Control de Cambios queda 100% silencioso.
    /// </summary>
    public static bool ControlCambiosActivo { get; set; }

    public JwtMessageHandler(ILocalStorageService localStorage, IJSRuntime js,
        JwtAuthStateProvider authProvider, NavigationManager nav)
    { _localStorage = localStorage; _js = js; _authProvider = authProvider; _nav = nav; }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Primero de memoria (sin interop). Solo si no hay nada en memoria — típicamente
        // tras recargar la página con F5, y aun así JwtAuthStateProvider ya suele haberlo
        // sembrado — se cae a localStorage, y ese valor se guarda en memoria para no volver
        // a pedirlo en cada request.
        var token = _tokenEnMemoria;
        if (string.IsNullOrWhiteSpace(token))
        {
            try
            {
                token = await _localStorage.GetItemAsync<string>("authToken");
                if (!string.IsNullOrWhiteSpace(token)) _tokenEnMemoria = token;
            }
            catch { /* sin token se sigue sin cabecera: el 401 lo maneja el bloque de abajo */ }
        }

        if (!string.IsNullOrWhiteSpace(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var sinSpinner = request.Options.TryGetValue(SinSpinnerKey, out var s) && s;
        var mostrarSpinner = !SplashActivo && !sinSpinner && !ControlCambiosActivo;
        if (mostrarSpinner) { try { await _js.InvokeVoidAsync("cursorBusy.show"); } catch { } }
        try
        {
            var response = await base.SendAsync(request, cancellationToken);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Solo se trata como sesión expirada si el token SÍ viajó y el servidor lo
                // rechazó. Si no había token, el usuario simplemente no tiene sesión, y de
                // eso ya se encarga el enrutador (AuthorizeRouteView lleva al login):
                // forzar la redirección desde acá lo expulsaba de la aplicación por el 401
                // de un sondeo de fondo — que es justo lo que ocurría al entrar recién
                // logueado. Se excluye además la propia pantalla de login para no entrar
                // en bucle con las llamadas que allí responden 401 legítimamente.
                var enLogin = _nav.Uri.Contains("/login", StringComparison.OrdinalIgnoreCase);
                var tokenViajo = !string.IsNullOrWhiteSpace(token);
                if (tokenViajo && !enLogin) await ManejarSesionExpiradaAsync();
            }
            return response;
        }
        finally
        {
            if (mostrarSpinner) { try { await _js.InvokeVoidAsync("cursorBusy.hide"); } catch { } }
        }
    }

    /// <summary>
    /// Limpia la sesión y redirige al login. Con muchos requests en paralelo (típico
    /// al cargar una pantalla, o varios sondeos venciendo casi al mismo tiempo), varios
    /// podrían recibir 401 casi juntos — el cooldown de abajo evita limpiar/redirigir más
    /// de una vez por esa ráfaga, pero SÍ permite reintentar en la ráfaga siguiente si esta
    /// no logró sacar al usuario de la página.
    /// </summary>
    private async Task ManejarSesionExpiradaAsync()
    {
        if ((DateTime.UtcNow - _ultimoIntentoRedirect).TotalSeconds < 5) return;
        _ultimoIntentoRedirect = DateTime.UtcNow;
        _tokenEnMemoria = null;
        try
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("usuarioSesion");
            await _localStorage.RemoveItemAsync("sesionBloqueada");
            var claves = await _localStorage.KeysAsync();
            foreach (var k in claves.Where(k => k.StartsWith("botonesTreeview_")).ToList())
                await _localStorage.RemoveItemAsync(k);
        }
        catch { }
        try { _authProvider.NotifyUserLogout(); } catch { }
        // Dos vías redundantes hacia el mismo destino: NavigateTo(forceLoad:true) es la forma
        // "correcta" en Blazor, pero no lanza excepción aunque el navegador por algún motivo no
        // llegue a navegar — con eso solo, un fallo silencioso ahí dejaba la app mostrando 401
        // en consola para siempre. location.replace por JS directo es la red de seguridad.
        try { _nav.NavigateTo("/login?expirada=1", forceLoad: true); } catch { }
        try { await _js.InvokeVoidAsync("eval", "location.replace('/login?expirada=1')"); } catch { }
    }
}
