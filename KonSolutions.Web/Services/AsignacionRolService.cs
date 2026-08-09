using System.Net.Http.Json;
using KONSolutions.Shared.Common;
using KONSolutions.Shared.Models.Seguridad;

namespace KONSolutions.Web.Services;

/// <summary>
/// Asignación (escritura) de roles a usuarios: SEGURIDAD.ROLES_USUARIO_USUARIOS.
/// Permite listar usuarios de un rol, agregar y quitar.
/// </summary>
public class AsignacionRolService
{
    private readonly HttpClient _http;
    private readonly ErrorLogService _errorLog;

    public AsignacionRolService(HttpClient http, ErrorLogService errorLog)
    { _http = http; _errorLog = errorLog; }

    /// <summary>Combo de roles.</summary>
    public async Task<List<ComboboxItem>> GetRolesComboAsync()
    {
        try
        {
            var r = await _http.GetFromJsonAsync<ApiResponse<List<ComboboxItem>>>(ApiRoutes.Seguridad.RolesCombobox);
            return r?.Data ?? new();
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "AsignacionRol", nameof(GetRolesComboAsync)); return new(); }
    }

    /// <summary>Combo de usuarios.</summary>
    public async Task<List<ComboboxItem>> GetUsuariosComboAsync()
    {
        try
        {
            var r = await _http.GetFromJsonAsync<ApiResponse<List<ComboboxItem>>>(ApiRoutes.Seguridad.UsuariosCombobox);
            return r?.Data ?? new();
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "AsignacionRol", nameof(GetUsuariosComboAsync)); return new(); }
    }

    /// <summary>Usuarios asignados a un rol.</summary>
    public async Task<List<RolAsignado>> GetUsuariosDeRolAsync(string codRol)
    {
        try
        {
            var r = await _http.GetFromJsonAsync<ApiResponse<List<RolAsignado>>>(
                $"{ApiRoutes.Seguridad.RolesUsuarioUsuarios}/por-rol/{Uri.EscapeDataString(codRol)}");
            return r?.Data ?? new();
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "AsignacionRol", nameof(GetUsuariosDeRolAsync)); return new(); }
    }

    /// <summary>Asigna un rol a un usuario.</summary>
    public async Task<(bool ok, string msg)> AsignarAsync(string codRol, string codUsuario)
    {
        var dto = new { COD_ROL_USUARIO = codRol, COD_USUARIO = codUsuario };
        return await Escribir(() => _http.PostAsJsonAsync(ApiRoutes.Seguridad.RolesUsuarioUsuarios, dto));
    }

    /// <summary>Quita un rol de un usuario.</summary>
    public async Task<(bool ok, string msg)> QuitarAsync(string codRol, string codUsuario)
        => await Escribir(() => _http.DeleteAsync(
            $"{ApiRoutes.Seguridad.RolesUsuarioUsuarios}/{Uri.EscapeDataString(codRol)}/{Uri.EscapeDataString(codUsuario)}"));

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
                await _errorLog.LogRechazoAsync(msgVacio, "AsignacionRol", "ESCRIBIR");
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
            await _errorLog.LogRechazoAsync(msg, "AsignacionRol", "ESCRIBIR");
            return (false, msg);
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "AsignacionRol", "ESCRIBIR"); return (false, ex.Message); }
    }
}
