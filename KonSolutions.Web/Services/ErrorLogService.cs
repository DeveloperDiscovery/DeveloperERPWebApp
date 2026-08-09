using Blazored.LocalStorage;
using System.Net.Http.Json;

namespace KONSolutions.Web.Services;

/// <summary>
/// Log de errores del frontend. Solo envía a la API si hay sesión activa.
/// </summary>
public class ErrorLogService
{
    private readonly HttpClient           _http;
    private readonly ILocalStorageService _localStorage;

    /// <summary>Corta el envío después de varios fallos seguidos.
    ///
    /// El log es diagnóstico, no funcionalidad: si el endpoint no está publicado —o la API no
    /// se reconstruyó y devuelve 404— cada llamada paga un viaje de red completo para nada.
    /// Con la traza del menú emitiendo un mensaje por paso, eso fue lo que convirtió una carga
    /// de menos de un segundo en veintidós.</summary>
    private const int FallosParaCortar = 3;
    private static int  _fallosSeguidos;
    private static bool _endpointCaido;

    public ErrorLogService(HttpClient http, ILocalStorageService localStorage)
    {
        _http         = http;
        _localStorage = localStorage;
    }

    /// <summary>Manda la entrada al log sin hacer esperar a quien llama.
    ///
    /// Nada de lo que hace este servicio es parte de la operación del usuario: si el POST tarda,
    /// tiene que tardar en segundo plano. Es fire-and-forget a propósito, y por eso también se
    /// traga cualquier excepción — una tarea sin observar que lanza no tiene a quién avisarle.</summary>
    private void EnviarEnSegundoPlano(object entry)
    {
        if (_endpointCaido) return;

        _ = Task.Run(async () =>
        {
            try
            {
                var resp = await _http.PostAsJsonAsync(ApiRoutes.Developer.ErrorLog, entry);
                if (resp.IsSuccessStatusCode) { _fallosSeguidos = 0; return; }

                // 401 al arranque es normal (el token todavía no está listo) y no cuenta como
                // endpoint caído; un 404 repetido sí: el controlador no está.
                if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized) return;
                RegistrarFallo($"HTTP {(int)resp.StatusCode}");
            }
            catch (Exception ex)
            {
                RegistrarFallo(ex.Message);
            }
        });
    }

    private static void RegistrarFallo(string motivo)
    {
        if (++_fallosSeguidos < FallosParaCortar) return;
        _endpointCaido = true;
        Console.Error.WriteLine(
            $"[LOG] Se deshabilita el envío al log remoto tras {FallosParaCortar} fallos seguidos ({motivo}). " +
            "Las trazas siguen en consola.");
    }

    public async Task LogAsync(Exception ex, string pagina, string metodo, string? parametros = null)
    {
        var entry = new
        {
            Programa   = "KONSolutions.Web",
            Clase      = pagina,
            Metodo     = metodo,
            TipoError  = "BLAZOR",
            Mensaje    = ex.Message,
            Detalle    = ex.ToString(),
            Parametros = parametros ?? string.Empty,
            Fecha      = DateTime.Now
        };

        Console.Error.WriteLine($"[ERROR] {pagina}.{metodo}: {ex.Message}");

        try
        {
            var sesion = await _localStorage.GetItemAsync<object>("usuarioSesion");
            if (sesion is null) return;
            EnviarEnSegundoPlano(entry);
        }
        catch { }
    }

    /// <summary>
    /// Registra un rechazo de operación devuelto por la API (Success=false) que NO llegó
    /// como excepción — sin esto, los rechazos de negocio/SQL nunca se centralizaban en el log.
    /// </summary>
    public async Task LogRechazoAsync(string mensaje, string pagina, string metodo, string? parametros = null)
    {
        var entry = new
        {
            Programa   = "KONSolutions.Web",
            Clase      = pagina,
            Metodo     = metodo,
            TipoError  = "RECHAZO",
            Mensaje    = mensaje,
            Detalle    = mensaje,
            Parametros = parametros ?? string.Empty,
            Fecha      = DateTime.Now
        };

        Console.Error.WriteLine($"[RECHAZO] {pagina}.{metodo}: {mensaje}");

        try
        {
            var sesion = await _localStorage.GetItemAsync<object>("usuarioSesion");
            if (sesion is null) return;
            EnviarEnSegundoPlano(entry);
        }
        catch { }
    }

    /// <summary>
    /// Registra un mensaje de traza informacional (sin excepción).
    /// Usa TipoError = "TRACE" para distinguirlo de errores reales.
    /// Escribe también en la consola del navegador para seguimiento en DevTools.
    /// </summary>
    /// <remarks>Devuelve una tarea ya completada: la traza se escribe en consola y el envío
    /// queda en segundo plano. El menú emite una traza por paso, así que este método está en el
    /// camino crítico del arranque y no puede costar un viaje de red — ni siquiera la lectura
    /// del localStorage, que es interop con JavaScript.
    ///
    /// La firma sigue siendo Task para no tocar los ~60 puntos que hacen await.</remarks>
    public Task TraceAsync(string mensaje, string detalle = "")
    {
        var now = DateTime.Now;
        Console.WriteLine($"[TRACE] {now:yyyy-MM-dd HH:mm:ss.fff} — {mensaje}");

        if (_endpointCaido) return Task.CompletedTask;

        var entry = new
        {
            Programa   = "KONSolutions.Web",
            Clase      = "MainLayout",
            Metodo     = mensaje,
            TipoError  = "TRACE",
            Mensaje    = mensaje,
            Detalle    = detalle,
            Parametros = string.Empty,
            Fecha      = now
        };

        _ = Task.Run(async () =>
        {
            try
            {
                // Aún sin sesión (ej. durante el login): solo consola.
                var sesion = await _localStorage.GetItemAsync<object>("usuarioSesion");
                if (sesion is null) return;
            }
            catch { return; }

            EnviarEnSegundoPlano(entry);
        });

        return Task.CompletedTask;
    }
}
