using System.Net.Http.Json;
using KONSolutions.Shared.Common;
using KONSolutions.Shared.Models.Entorno;

namespace KONSolutions.Web.Services;

/// <summary>
/// Cliente del Dashboard de Monitoreo de BD. Cada método pega a un endpoint del
/// DashboardBdController, que a su vez ejecuta un SP PROC_ENTORNO_DASHBOARD_BD_*.
/// El trace se sirve del buffer en memoria del API (no pega a la BD).
/// </summary>
public class DashboardBdService
{
    private readonly HttpClient _http;
    private readonly ErrorLogService _errorLog;
    public DashboardBdService(HttpClient http, ErrorLogService errorLog) { _http = http; _errorLog = errorLog; }

    private string Base => ApiRoutes.Entorno.DashboardBd;

    private async Task<T?> GetUno<T>(string path)
    {
        try { return (await _http.GetFromJsonAsync<ApiResponse<T>>($"{Base}/{path}"))!.Data; }
        catch { return default; }
    }

    private async Task<List<T>> GetLista<T>(string path)
    {
        try { return (await _http.GetFromJsonAsync<ApiResponse<List<T>>>($"{Base}/{path}"))?.Data ?? new(); }
        catch { return new(); }
    }

    public Task<DashboardBdResumen?>            GetResumenAsync()        => GetUno<DashboardBdResumen>("resumen");
    public Task<List<DashboardBdSesion>>        GetSesionesAsync()       => GetLista<DashboardBdSesion>("sesiones");
    public Task<List<DashboardBdBloqueo>>       GetBloqueosAsync()       => GetLista<DashboardBdBloqueo>("bloqueos");
    public Task<List<DashboardBdConsulta>>      GetConsultasAsync(int top = 20) => GetLista<DashboardBdConsulta>($"consultas?top={top}");
    public Task<List<DashboardBdWait>>          GetWaitsAsync(int top = 15)     => GetLista<DashboardBdWait>($"waits?top={top}");
    public Task<List<DashboardBdTabla>>         GetTablasAsync(int top = 30)    => GetLista<DashboardBdTabla>($"tablas?top={top}");
    public Task<List<DashboardBdFragmentacion>> GetFragmentacionAsync()  => GetLista<DashboardBdFragmentacion>("fragmentacion");
    public Task<List<DashboardBdIndiceFaltante>>GetIndicesFaltantesAsync(int top = 20) => GetLista<DashboardBdIndiceFaltante>($"indices-faltantes?top={top}");
    public Task<List<DashboardBdArchivo>>       GetArchivosAsync()       => GetLista<DashboardBdArchivo>("archivos");
    public Task<List<DashboardBdBackup>>        GetBackupsAsync()        => GetLista<DashboardBdBackup>("backups");
    public Task<DashboardBdMemoria?>            GetMemoriaAsync()        => GetUno<DashboardBdMemoria>("memoria");
    public Task<List<DashboardBdIoLatencia>>    GetIoLatenciaAsync()     => GetLista<DashboardBdIoLatencia>("io-latencia");
    public Task<List<DashboardBdProcedimiento>> GetProcedimientosAsync(int top = 20) => GetLista<DashboardBdProcedimiento>($"procedimientos?top={top}");
    public Task<List<SpTraceEntry>>             GetTraceAsync(int top = 200)    => GetLista<SpTraceEntry>($"trace?top={top}");

    /// <summary>CHECKPOINT + SHRINKFILE del log. Devuelve MB antes/después, o error con el motivo real.</summary>
    public async Task<(DashboardBdLimpiezaLog? data, string? error)> LimpiarLogAsync(int objetivoMb = 512)
        => await Post<DashboardBdLimpiezaLog>($"limpiar-log?objetivoMb={objetivoMb}", "LIMPIAR_LOG");

    /// <summary>Genera un backup FULL/DIF/LOG en la carpeta por defecto de la instancia.</summary>
    public async Task<(DashboardBdBackupResultado? data, string? error)> GenerarBackupAsync(string tipo = "FULL")
        => await Post<DashboardBdBackupResultado>($"backup?tipo={tipo}", "BACKUP");

    /// <summary>KILL de la sesión indicada (el SP protege sesiones de sistema).</summary>
    public async Task<(DashboardBdKillResultado? data, string? error)> KillSesionAsync(int numSesion)
        => await Post<DashboardBdKillResultado>($"kill-sesion?numSesion={numSesion}", "KILL_SESION");

    /// <summary>
    /// Operaciones críticas de administración de BD (backup/kill/limpieza de log): antes tragaban
    /// cualquier falla en silencio (catch vacío → null), sin mensaje real ni registro en el log
    /// centralizado — justo donde más importa saber qué pasó. Ahora extrae el mensaje real
    /// (SQL/API) y lo centraliza con LogRechazoAsync/LogAsync.
    /// </summary>
    private async Task<(T? data, string? error)> Post<T>(string pathQuery, string evento)
    {
        try
        {
            var resp = await _http.PostAsync($"{Base}/{pathQuery}", null);
            var raw = await resp.Content.ReadAsStringAsync();

            ApiResponse<T>? r = null;
            try
            {
                r = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<T>>(raw,
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (System.Text.Json.JsonException) { /* cuerpo no es ApiResponse; se maneja abajo */ }

            if (resp.IsSuccessStatusCode && r is not null && r.Success) return (r.Data, null);

            var msg = ApiErrorHelper.ExtraerMensaje(raw, r?.Message ?? "", (int)resp.StatusCode);
            await _errorLog.LogRechazoAsync(msg, "DashboardMonitoreoBD", evento);
            return (default, msg);
        }
        catch (Exception ex)
        {
            await _errorLog.LogAsync(ex, "DashboardMonitoreoBD", evento);
            return (default, ex.Message);
        }
    }
}
