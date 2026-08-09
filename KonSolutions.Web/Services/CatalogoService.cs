using System.Net.Http.Json;
using KONSolutions.Shared.Common;

namespace KONSolutions.Web.Services;

/// <summary>
/// Servicio CRUD genérico para catálogos simples (lectura, combobox, guardar, eliminar).
/// Reutilizable por Perfiles, Roles, ComplejidadPassword, Aplicaciones.
/// TEntidad = modelo de lectura, TDto = modelo de escritura.
/// </summary>
public class CatalogoService<TEntidad, TDto>
{
    private readonly HttpClient _http;
    private readonly AccessLogService _accessLog;
    private readonly ErrorLogService _errorLog;
    private readonly string _ruta;
    private readonly string _modulo;

    public CatalogoService(HttpClient http, AccessLogService accessLog, ErrorLogService errorLog,
        string ruta, string modulo)
    { _http = http; _accessLog = accessLog; _errorLog = errorLog; _ruta = ruta; _modulo = modulo; }

    public async Task<List<TEntidad>> GetAllAsync()
    {
        try
        {
            await _accessLog.LogEventoAsync(_modulo, _modulo, "SHOWALL");
            var r = await _http.GetFromJsonAsync<ApiResponse<List<TEntidad>>>(_ruta);
            return r?.Data ?? new();
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, _modulo, nameof(GetAllAsync)); return new(); }
    }

    /// <summary>Búsqueda usando el endpoint /search (SP _SEARCH).</summary>
    public async Task<List<TEntidad>> SearchAsync(string termino)
    {
        if (string.IsNullOrWhiteSpace(termino)) return await GetAllAsync();
        try
        {
            await _accessLog.LogEventoAsync(_modulo, _modulo, "SEARCH", new { termino });
            var r = await _http.GetFromJsonAsync<ApiResponse<List<TEntidad>>>(
                $"{_ruta}/search?search={Uri.EscapeDataString(termino)}");
            return r?.Data ?? new();
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, _modulo, nameof(SearchAsync)); return new(); }
    }

    public async Task<List<ComboboxItem>> GetComboboxAsync()
    {
        try
        {
            var r = await _http.GetFromJsonAsync<ApiResponse<List<ComboboxItem>>>($"{_ruta}/combobox");
            return r?.Data ?? new();
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, _modulo, nameof(GetComboboxAsync)); return new(); }
    }

    public async Task<TEntidad?> GetByIdAsync(string id)
    {
        try
        {
            var r = await _http.GetFromJsonAsync<ApiResponse<TEntidad>>($"{_ruta}/{Uri.EscapeDataString(id)}");
            return r is not null ? r.Data : default;
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, _modulo, nameof(GetByIdAsync), id); return default; }
    }

    /// <summary>Guarda el registro.
    ///
    /// <paramref name="esNuevo"/> decide el verbo: true = alta (POST), false = modificación
    /// (PUT). Se dejó como opcional —y en null manda POST, como siempre— porque la separación
    /// de INSERT y UPDATE avanza módulo por módulo: las pantallas cuyo endpoint todavía no
    /// tiene PUT siguen llamando sin el parámetro y funcionan igual.
    ///
    /// Antes esto no importaba, porque el SP hacía upsert y el POST servía para las dos cosas.
    /// Ahora un POST sobre una clave existente es rechazado, así que la intención tiene que
    /// viajar desde la pantalla, que es la única que la conoce.</summary>
    public async Task<(bool ok, string msg)> GuardarAsync(TDto dto, bool? esNuevo = null)
    {
        var r = await GuardarDetalladoAsync(dto, esNuevo);
        return (r.Ok, r.Msg);
    }

    public async Task<(bool ok, string msg)> EliminarAsync(string id)
    {
        var r = await EscribirDetallado(() => _http.DeleteAsync($"{_ruta}/{Uri.EscapeDataString(id)}"), "DELETE", id);
        return (r.Ok, r.Msg);
    }

    /// <summary>Variante con detalle técnico completo (N.º/procedimiento/línea/sentencia SQL) —
    /// usar cuando el diálogo muestra ErrorSqlBox.razor en vez de solo un Snackbar.</summary>
    public Task<EscribirResultado> GuardarDetalladoAsync(TDto dto, bool? esNuevo = null)
        => EscribirDetallado(() => esNuevo == false
                                    ? _http.PutAsJsonAsync(_ruta, dto)
                                    : _http.PostAsJsonAsync(_ruta, dto), "GUARDAR");

    public Task<EscribirResultado> EliminarDetalladoAsync(string id)
        => EscribirDetallado(() => _http.DeleteAsync($"{_ruta}/{Uri.EscapeDataString(id)}"), "DELETE", id);

    private async Task<EscribirResultado> EscribirDetallado(Func<Task<HttpResponseMessage>> accion, string evento, string? id = null)
    {
        try
        {
            await _accessLog.LogEventoAsync(_modulo, _modulo, evento, id is null ? null : new { id });
            var resp = await accion();
            var raw = await resp.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(raw))
            {
                if (resp.IsSuccessStatusCode) return new(true, "Operación realizada.");
                var msgVacio = ApiErrorHelper.ExtraerMensaje("", "", (int)resp.StatusCode);
                await _errorLog.LogRechazoAsync(msgVacio, _modulo, evento, id);
                return new(false, msgVacio);
            }

            ApiResponse<WriteResult>? r = null;
            try
            {
                r = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<WriteResult>>(raw,
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (System.Text.Json.JsonException) { /* cuerpo no es ApiResponse; se maneja abajo */ }

            if (r is not null && r.Success)
                return new(true, string.IsNullOrWhiteSpace(r.Message) ? "Operación realizada." : r.Message);

            if (resp.IsSuccessStatusCode && r is null) return new(true, "Operación realizada.");

            var msg = ApiErrorHelper.ExtraerMensaje(raw, r?.Message ?? "", (int)resp.StatusCode);
            await _errorLog.LogRechazoAsync(msg, _modulo, evento, id);
            return new(false, msg, r?.ErrorNumber, r?.ErrorProcedure, r?.ErrorLine, r?.SqlSentence);
        }
        catch (Exception ex)
        {
            await _errorLog.LogAsync(ex, _modulo, evento);
            return new(false, ex.Message);
        }
    }
}

/// <summary>Resultado detallado de una escritura (Guardar/Eliminar) — incluye el detalle
/// técnico del error (SQL) cuando la API lo trae, para mostrar en ErrorSqlBox.razor.</summary>
public record EscribirResultado(bool Ok, string Msg, int? ErrorNumber = null, string? ErrorProcedure = null, int? ErrorLine = null, string? SqlSentence = null);
