using System.Net.Http.Json;
using KONSolutions.Shared.Common;
using KONSolutions.Shared.Models.Seguridad;

namespace KONSolutions.Web.Services;

/// <summary>
/// Servicio de USUARIOS: CRUD, búsqueda y operaciones especiales.
/// Cada operación registra acceso/evento en el AccessLogService.
/// </summary>
public class UsuarioService
{
    private readonly HttpClient _http;
    private readonly AccessLogService _accessLog;
    private readonly ErrorLogService _errorLog;

    public UsuarioService(HttpClient http, AccessLogService accessLog, ErrorLogService errorLog)
    { _http = http; _accessLog = accessLog; _errorLog = errorLog; }

    public async Task<List<Usuario>> GetAllAsync()
    {
        try
        {
            await _accessLog.LogEventoAsync("SEGURIDAD", "Usuarios", "SHOWALL");
            var r = await _http.GetFromJsonAsync<ApiResponse<List<Usuario>>>(ApiRoutes.Seguridad.Usuarios);
            return r?.Data ?? new();
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "Usuarios", nameof(GetAllAsync)); return new(); }
    }

    /// <summary>Búsqueda con PROC_SEGURIDAD_USUARIOS_SEARCH.</summary>
    public async Task<List<Usuario>> SearchAsync(string termino)
    {
        try
        {
            await _accessLog.LogEventoAsync("SEGURIDAD", "Usuarios", "SEARCH", new { termino });
            var r = await _http.GetFromJsonAsync<ApiResponse<List<Usuario>>>(
                $"{ApiRoutes.Seguridad.UsuariosSearch}?search={Uri.EscapeDataString(termino)}");
            return r?.Data ?? new();
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "Usuarios", nameof(SearchAsync), termino); return new(); }
    }

    public async Task<Usuario?> GetByIdAsync(string codUsuario)
    {
        try
        {
            await _accessLog.LogEventoAsync("SEGURIDAD", "Usuarios", "SHOWBYID", new { codUsuario });
            var r = await _http.GetFromJsonAsync<ApiResponse<Usuario>>($"{ApiRoutes.Seguridad.Usuarios}/{Uri.EscapeDataString(codUsuario)}");
            return r?.Data;
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "Usuarios", nameof(GetByIdAsync), codUsuario); return null; }
    }

    public async Task<(bool ok, string msg)> InsertAsync(UsuarioInsertDto dto)
        => await Escribir(() => _http.PostAsJsonAsync(ApiRoutes.Seguridad.Usuarios, dto), "INSERT", new { dto.COD_USUARIO });

    public async Task<(bool ok, string msg)> UpdateAsync(string cod, UsuarioUpdateDto dto)
        => await Escribir(() => _http.PostAsJsonAsync(ApiRoutes.Seguridad.Usuarios, dto), "UPDATE", new { cod });

    public async Task<(bool ok, string msg)> DeleteAsync(string cod)
        => await Escribir(() => _http.DeleteAsync($"{ApiRoutes.Seguridad.Usuarios}/{Uri.EscapeDataString(cod)}"), "DELETE", new { cod });

    /// <summary>Resetear intentos (PROC_SEGURIDAD_USUARIOS_RESETINTENTOS).</summary>
    public async Task<(bool ok, string msg)> ResetIntentosAsync(string cod)
        => await Escribir(() => _http.PostAsJsonAsync(ApiRoutes.Seguridad.UsuariosResetIntentos, new { COD_USUARIO = cod }),
               "RESETINTENTOS", new { cod });

    /// <summary>Cambiar contraseña (PROC_SEGURIDAD_USUARIOS_PASSWORD).</summary>
    public async Task<(bool ok, string msg)> CambiarPasswordAsync(CambiarPasswordRequest req)
        => await Escribir(() => _http.PostAsJsonAsync(ApiRoutes.Seguridad.UsuariosPassword, req),
               "PASSWORD", new { req.COD_USUARIO });

    /// <summary>
    /// Resetear contraseña (PROC_SEGURIDAD_USUARIOS_PASSWORDRESET).
    /// Llama a POST /seguridad/usuarios/password-reset; deja el password en NULL.
    /// </summary>
    public async Task<(bool ok, string msg)> ResetPasswordAsync(ResetPasswordRequest req)
        => await Escribir(() => _http.PostAsJsonAsync(ApiRoutes.Seguridad.UsuariosPasswordReset, req),
               "PASSWORDRESET", new { req.COD_USUARIO });

    /// <summary>Cambiar estado (PROC_SEGURIDAD_USUARIOS_ESTADOS).</summary>
    public async Task<(bool ok, string msg)> CambiarEstadoAsync(CambiarEstadoRequest req)
        => await Escribir(() => _http.PostAsJsonAsync(ApiRoutes.Seguridad.UsuariosEstados, req),
               "ESTADOS", new { req.COD_USUARIO, req.COD_ESTADO });

    private async Task<(bool, string)> Escribir(Func<Task<HttpResponseMessage>> accion, string evento, object parametros)
    {
        try
        {
            await _accessLog.LogEventoAsync("SEGURIDAD", "Usuarios", evento, parametros);
            var resp = await accion();

            // Leer el cuerpo como texto primero (puede venir vacío si el SP no devuelve nada)
            var raw = await resp.Content.ReadAsStringAsync();

            // Si el cuerpo viene vacío: el éxito se decide por el código HTTP.
            if (string.IsNullOrWhiteSpace(raw))
            {
                if (resp.IsSuccessStatusCode) return (true, "Operación realizada correctamente.");
                var msgVacio = ApiErrorHelper.ExtraerMensaje("", "", (int)resp.StatusCode);
                await _errorLog.LogRechazoAsync(msgVacio, "Usuarios", evento, System.Text.Json.JsonSerializer.Serialize(parametros));
                return (false, msgVacio);
            }

            // Hay cuerpo: intentar interpretarlo como ApiResponse<WriteResult>.
            ApiResponse<WriteResult>? r = null;
            try
            {
                r = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<WriteResult>>(raw,
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (System.Text.Json.JsonException) { /* cuerpo no es ApiResponse; se maneja abajo */ }

            if (r is not null && r.Success)
                return (true, string.IsNullOrWhiteSpace(r.Message) ? "Operación realizada." : r.Message);

            if (resp.IsSuccessStatusCode && r is null) return (true, "Operación realizada correctamente.");

            var msg = ApiErrorHelper.ExtraerMensaje(raw, r?.Message ?? "", (int)resp.StatusCode);
            await _errorLog.LogRechazoAsync(msg, "Usuarios", evento, System.Text.Json.JsonSerializer.Serialize(parametros));
            return (false, msg);
        }
        catch (Exception ex)
        {
            await _errorLog.LogAsync(ex, "Usuarios", evento, System.Text.Json.JsonSerializer.Serialize(parametros));
            return (false, ex.Message);
        }
    }
}
