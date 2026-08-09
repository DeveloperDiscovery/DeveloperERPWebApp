using System.Net.Http.Json;
using KONSolutions.Shared.Common;
using KONSolutions.Shared.Models.Estandar;

namespace KONSolutions.Web.Services;

/// <summary>
/// Servicio para ESTANDAR.ESTADOS. Los estados son hijos de un TipoEstado,
/// por eso se cargan filtrados por COD_TIPO_ESTADO.
/// </summary>
public class EstadoService
{
    private readonly HttpClient _http;
    private readonly AccessLogService _accessLog;
    private readonly ErrorLogService _errorLog;

    public EstadoService(HttpClient http, AccessLogService accessLog, ErrorLogService errorLog)
    { _http = http; _accessLog = accessLog; _errorLog = errorLog; }

    /// <summary>Estados de un tipo específico (sección hija).</summary>
    public async Task<List<Estado>> GetByTipoAsync(int codTipoEstado)
    {
        try
        {
            await _accessLog.LogEventoAsync("ESTANDAR", "Estados", "SHOWALL", new { codTipoEstado });
            var r = await _http.GetFromJsonAsync<ApiResponse<List<Estado>>>(
                $"{ApiRoutes.Estandar.EstadosPorTipo}/{codTipoEstado}");
            return r?.Data ?? new();
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "Estados", nameof(GetByTipoAsync)); return new(); }
    }

    /// <summary>Combo de Tipos de Motivos (acepta null en el formulario).</summary>
    public async Task<List<ComboboxItem>> GetTiposMotivosAsync()
    {
        try
        {
            // Usa el SHOWALL de tipos de motivos (devuelve COD_TIPO_MOTIVO, DES_TIPO_MOTIVO, IMG_PICTURE)
            var r = await _http.GetFromJsonAsync<ApiResponse<List<TipoMotivo>>>(
                ApiRoutes.Estandar.TiposMotivos);
            return r?.Data?.Select(t => new ComboboxItem
            {
                Id = t.COD_TIPO_MOTIVO?.ToString() ?? "",
                Texto = t.DES_TIPO_MOTIVO ?? ""
            }).ToList() ?? new();
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "Estados", nameof(GetTiposMotivosAsync)); return new(); }
    }

    /// <summary>Importa un Estado Estándar al tipo indicado (SP _FROM_ESTADOS_ESTANDAR).</summary>
    public async Task<(bool ok, string msg)> ImportarDeEstandarAsync(int codTipoEstado, int codEstadoEstandar)
        => await Escribir(() => _http.PostAsync(
            $"{ApiRoutes.Estandar.EstadosImportEstandar}/{codTipoEstado}/{codEstadoEstandar}", null), "IMPORT");

    public async Task<(bool ok, string msg)> GuardarAsync(EstadoDto dto)
        => await Escribir(() => _http.PostAsJsonAsync(ApiRoutes.Estandar.Estados, dto), "GUARDAR");

    public async Task<(bool ok, string msg)> EliminarAsync(int codTipoEstado, int codEstado)
        => await Escribir(() => _http.DeleteAsync($"{ApiRoutes.Estandar.Estados}/{codTipoEstado}/{codEstado}"), "DELETE");

    private async Task<(bool, string)> Escribir(Func<Task<HttpResponseMessage>> accion, string evento)
    {
        try
        {
            await _accessLog.LogEventoAsync("ESTANDAR", "Estados", evento);
            var resp = await accion();
            var raw = await resp.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(raw))
            {
                if (resp.IsSuccessStatusCode) return (true, "Operación realizada.");
                var msgVacio = ApiErrorHelper.ExtraerMensaje("", "", (int)resp.StatusCode);
                await _errorLog.LogRechazoAsync(msgVacio, "Estados", evento);
                return (false, msgVacio);
            }

            ApiResponse<WriteResult>? r = null;
            try
            {
                r = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<WriteResult>>(raw,
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (System.Text.Json.JsonException) { /* cuerpo no es ApiResponse; se maneja abajo */ }

            if (r is not null && r.Success)
                return (true, string.IsNullOrWhiteSpace(r.Message) ? "Operación realizada." : r.Message);

            if (resp.IsSuccessStatusCode && r is null) return (true, "Operación realizada.");

            var msg = ApiErrorHelper.ExtraerMensaje(raw, r?.Message ?? "", (int)resp.StatusCode);
            await _errorLog.LogRechazoAsync(msg, "Estados", evento);
            return (false, msg);
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "Estados", evento); return (false, ex.Message); }
    }
}
