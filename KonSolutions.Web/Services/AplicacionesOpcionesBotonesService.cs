using System.Net.Http.Json;
using KONSolutions.Shared.Common;
using KONSolutions.Shared.Models.Seguridad;

namespace KONSolutions.Web.Services;

/// <summary>
/// Servicio para CRUD de botones de opciones de aplicaciones.
/// Clave compuesta: COD_APLICACION + COD_OPCION_APLICACION + COD_BOTON_OPCION.
/// </summary>
public class AplicacionesOpcionesBotonesService
{
    private readonly HttpClient        _http;
    private readonly AccessLogService  _accessLog;
    private readonly ErrorLogService   _errorLog;
    private const string Modulo = "APLICACIONES_OPCIONES_BOTONES";

    public AplicacionesOpcionesBotonesService(HttpClient http, AccessLogService accessLog, ErrorLogService errorLog)
    { _http = http; _accessLog = accessLog; _errorLog = errorLog; }

    /// <summary>Lista los botones de una opción específica (COD_APLICACION + COD_OPCION_APLICACION).</summary>
    public async Task<List<AplicacionesOpcionesBotones>> GetByOpcionAsync(string codAplicacion, string codOpcionAplicacion)
    {
        try
        {
            await _accessLog.LogEventoAsync(Modulo, Modulo, "SHOWALL", new { codAplicacion, codOpcionAplicacion });
            var url = $"{ApiRoutes.Seguridad.AplicacionesOpcionesBotones}" +
                      $"?codAplicacion={Uri.EscapeDataString(codAplicacion)}" +
                      $"&codOpcionAplicacion={Uri.EscapeDataString(codOpcionAplicacion)}";
            var r = await _http.GetFromJsonAsync<ApiResponse<List<AplicacionesOpcionesBotones>>>(url);
            return r?.Data ?? new();
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, Modulo, nameof(GetByOpcionAsync)); return new(); }
    }

    public async Task<(bool ok, string msg)> GuardarAsync(AplicacionesOpcionesBotonesInsertDto dto)
    {
        var r = await EscribirDetallado(() => _http.PostAsJsonAsync(ApiRoutes.Seguridad.AplicacionesOpcionesBotones, dto), "GUARDAR");
        return (r.ok, r.msg);
    }

    public async Task<(bool ok, string msg)> EliminarAsync(string codAplicacion, string codOpcionAplicacion, string codBotonOpcion)
    {
        var r = await EscribirDetallado(() => _http.DeleteAsync(
            $"{ApiRoutes.Seguridad.AplicacionesOpcionesBotones}/{Uri.EscapeDataString(codAplicacion)}" +
            $"/{Uri.EscapeDataString(codOpcionAplicacion)}/{Uri.EscapeDataString(codBotonOpcion)}"), "DELETE");
        return (r.ok, r.msg);
    }

    /// <summary>Variante con detalle técnico del error (N.º/procedimiento/línea de SQL Server) —
    /// usar cuando el diálogo muestra ErrorSqlBox.razor en vez de solo un Snackbar.</summary>
    public Task<(bool ok, string msg, int? errNum, string? errProc, int? errLine)> GuardarDetalladoAsync(AplicacionesOpcionesBotonesInsertDto dto)
        => EscribirDetallado(() => _http.PostAsJsonAsync(ApiRoutes.Seguridad.AplicacionesOpcionesBotones, dto), "GUARDAR");

    public Task<(bool ok, string msg, int? errNum, string? errProc, int? errLine)> EliminarDetalladoAsync(string codAplicacion, string codOpcionAplicacion, string codBotonOpcion)
        => EscribirDetallado(() => _http.DeleteAsync(
            $"{ApiRoutes.Seguridad.AplicacionesOpcionesBotones}/{Uri.EscapeDataString(codAplicacion)}" +
            $"/{Uri.EscapeDataString(codOpcionAplicacion)}/{Uri.EscapeDataString(codBotonOpcion)}"), "DELETE");

    private async Task<(bool ok, string msg, int? errNum, string? errProc, int? errLine)> EscribirDetallado(Func<Task<HttpResponseMessage>> accion, string evento)
    {
        try
        {
            await _accessLog.LogEventoAsync(Modulo, Modulo, evento);
            var resp = await accion();
            var raw = await resp.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(raw))
            {
                if (resp.IsSuccessStatusCode) return (true, "Operación realizada.", null, null, null);
                var msgVacio = ApiErrorHelper.ExtraerMensaje("", "", (int)resp.StatusCode);
                await _errorLog.LogRechazoAsync(msgVacio, Modulo, evento);
                return (false, msgVacio, null, null, null);
            }

            ApiResponse<WriteResult>? r = null;
            try
            {
                r = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<WriteResult>>(raw,
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (System.Text.Json.JsonException) { /* cuerpo no es ApiResponse; se maneja abajo */ }

            if (r is not null && r.Success)
                return (true, string.IsNullOrWhiteSpace(r.Message) ? "Operación realizada." : r.Message, null, null, null);

            if (resp.IsSuccessStatusCode && r is null) return (true, "Operación realizada.", null, null, null);

            var msg = ApiErrorHelper.ExtraerMensaje(raw, r?.Message ?? "", (int)resp.StatusCode);
            await _errorLog.LogRechazoAsync(msg, Modulo, evento);
            return (false, msg, r?.ErrorNumber, r?.ErrorProcedure, r?.ErrorLine);
        }
        catch (Exception ex)
        {
            await _errorLog.LogAsync(ex, Modulo, evento);
            return (false, ex.Message, null, null, null);
        }
    }
}
