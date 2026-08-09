using System.Net.Http.Json;
using KONSolutions.Shared.Common;

namespace KONSolutions.Web.Services;

/// <summary>
/// Sondea el endpoint de control de cambios cada cierto intervalo y avisa
/// a las grillas suscritas cuando su tabla cambió en la BD.
///
/// Uso en una pantalla:
///   - En OnInitialized: Cambios.Suscribir("SEGURIDAD.USUARIOS", Load);
///   - En Dispose:       Cambios.Desuscribir("SEGURIDAD.USUARIOS", Load);
/// </summary>
public class ControlCambiosClientService : IDisposable
{
    private readonly HttpClient _http;
    private readonly ErrorLogService _errorLog;

    private System.Threading.Timer? _timer;
    private Dictionary<string, long> _versiones = new(StringComparer.OrdinalIgnoreCase);

    // tabla → callbacks de refresco
    private readonly Dictionary<string, List<Func<Task>>> _suscriptores = new(StringComparer.OrdinalIgnoreCase);

    // Intervalo de sondeo en segundos (configurable). Por defecto 10s.
    public int IntervaloSegundos { get; private set; } = 10;
    public bool Activo { get; private set; }

    public ControlCambiosClientService(HttpClient http, ErrorLogService errorLog)
    { _http = http; _errorLog = errorLog; }

    /// <summary>Arranca el sondeo (llamar tras login).</summary>
    public void Iniciar(int? intervaloSegundos = null)
    {
        if (intervaloSegundos is > 0) IntervaloSegundos = intervaloSegundos.Value;
        Detener();
        Activo = true;
        // Delay inicial de 2s para asegurar que el token esté disponible antes del primer sondeo
        _timer = new System.Threading.Timer(async _ => await Sondear(), null,
            TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(IntervaloSegundos));
    }

    /// <summary>Cambia el intervalo en caliente (desde Configuración).</summary>
    public void CambiarIntervalo(int segundos)
    {
        if (segundos <= 0) return;
        IntervaloSegundos = segundos;
        if (Activo) Iniciar(segundos);
    }

    public void Detener()
    {
        _timer?.Dispose();
        _timer = null;
        Activo = false;
    }

    public void Suscribir(string tabla, Func<Task> onCambio)
    {
        if (!_suscriptores.TryGetValue(tabla, out var lista))
            _suscriptores[tabla] = lista = new();
        if (!lista.Contains(onCambio)) lista.Add(onCambio);
    }

    public void Desuscribir(string tabla, Func<Task> onCambio)
    {
        if (_suscriptores.TryGetValue(tabla, out var lista))
            lista.Remove(onCambio);
    }

    // Evita que un tick del timer arranque otra ronda de Sondear() mientras la anterior
    // sigue en vuelo (ej. red lenta, o varias grillas suscritas recargando a la vez) —
    // sin esto, los sondeos se apilan y compiten por el único hilo de UI en Blazor WASM.
    private bool _sondeando;

    private async Task Sondear()
    {
        if (_sondeando) return;
        _sondeando = true;
        try
        {
            using var req = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Entorno.ControlCambios);
            req.Options.Set(JwtMessageHandler.SinSpinnerKey, true);
            var resp = await _http.SendAsync(req);
            if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized
             || resp.StatusCode == System.Net.HttpStatusCode.Forbidden)
                return; // sin token válido: esperar al siguiente ciclo sin loguear error

            resp.EnsureSuccessStatusCode();
            var r = await resp.Content.ReadFromJsonAsync<ApiResponse<Dictionary<string, long>>>();
            var nuevas = r?.Data;
            if (nuevas is null) return;

            // Detectar qué tablas cambiaron de versión
            var cambiadas = new List<string>();
            foreach (var kv in nuevas)
            {
                var anterior = _versiones.TryGetValue(kv.Key, out var v) ? v : -1;
                // -1 = primera lectura: no dispara refresco (solo memoriza)
                if (anterior >= 0 && kv.Value != anterior)
                    cambiadas.Add(kv.Key);
            }
            _versiones = new Dictionary<string, long>(nuevas, StringComparer.OrdinalIgnoreCase);

            // Notificar a los suscriptores de cada tabla cambiada. Se marca ControlCambiosActivo
            // para que los Load() disparados por estos callbacks tampoco enciendan el spinner
            // global — todo el proceso de Control de Cambios debe ser silencioso.
            JwtMessageHandler.ControlCambiosActivo = true;
            try
            {
                foreach (var tabla in cambiadas)
                {
                    if (_suscriptores.TryGetValue(tabla, out var lista))
                        foreach (var cb in lista.ToList())
                            await cb();
                }
            }
            finally { JwtMessageHandler.ControlCambiosActivo = false; }
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "ControlCambios", nameof(Sondear)); }
        finally { _sondeando = false; }
    }

    public void Dispose() => Detener();
}
