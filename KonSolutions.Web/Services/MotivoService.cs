using System.Net.Http.Json;
using KONSolutions.Shared.Common;
using KONSolutions.Shared.Models.Estandar;

namespace KONSolutions.Web.Services;

/// <summary>Motivos (hijos de un TipoMotivo). Se cargan filtrados por COD_TIPO_MOTIVO.</summary>
public class MotivoService
{
    private readonly HttpClient _http;
    private readonly AccessLogService _accessLog;
    private readonly ErrorLogService _errorLog;

    public MotivoService(HttpClient http, AccessLogService accessLog, ErrorLogService errorLog)
    { _http = http; _accessLog = accessLog; _errorLog = errorLog; }

    public async Task<List<Motivo>> GetByTipoAsync(int codTipoMotivo)
    {
        try
        {
            var r = await _http.GetFromJsonAsync<ApiResponse<List<Motivo>>>(
                $"{ApiRoutes.Estandar.MotivosPorTipo}/{codTipoMotivo}");
            return r?.Data ?? new();
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "Motivos", nameof(GetByTipoAsync)); return new(); }
    }

    /// <summary>Alta o modificación, según EsNuevo — que el diálogo ya viene calculando.
    ///
    /// Antes esto siempre hacía POST. Con el SP de alta separado del de modificación, editar un
    /// motivo terminaba creando otro: el código es autogenerado, así que no chocaba contra la
    /// clave, simplemente se duplicaba la fila.
    ///
    /// La decisión se toma acá y no en el diálogo a propósito: el DTO ya lleva la intención, y
    /// dejarla en un solo lugar evita que la próxima pantalla que use este servicio se olvide.</summary>
    public async Task<(bool ok, string msg)> GuardarAsync(MotivoDto dto)
        => await Escribir(() => dto.EsNuevo
                                ? _http.PostAsJsonAsync(ApiRoutes.Estandar.Motivos, dto)
                                : _http.PutAsJsonAsync(ApiRoutes.Estandar.Motivos, dto));

    public async Task<(bool ok, string msg)> EliminarAsync(int codTipoMotivo, int codMotivo)
        => await Escribir(() => _http.DeleteAsync($"{ApiRoutes.Estandar.Motivos}/{codTipoMotivo}/{codMotivo}"));

    private async Task<(bool, string)> Escribir(Func<Task<HttpResponseMessage>> accion)
    {
        try
        {
            var resp = await accion();
            var raw = await resp.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(raw))
            {
                if (resp.IsSuccessStatusCode) return (true, "Operación realizada.");
                var msgVacio = ApiErrorHelper.ExtraerMensaje("", "", (int)resp.StatusCode);
                await _errorLog.LogRechazoAsync(msgVacio, "Motivos", "ESCRIBIR");
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
            await _errorLog.LogRechazoAsync(msg, "Motivos", "ESCRIBIR");
            return (false, msg);
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "Motivos", "ESCRIBIR"); return (false, ex.Message); }
    }
}
