using System.Net.Http.Json;
using KONSolutions.Shared.Common;
using KONSolutions.Shared.Models.Seguridad;

namespace KONSolutions.Web.Services;

/// <summary>
/// Consume las vistas de asignación (roles, opciones, botones) que se muestran
/// en los tabs de la pantalla de Usuarios. Usa los SP _SHOWALL existentes.
/// </summary>
public class AsignacionService
{
    private readonly HttpClient _http;
    private readonly ErrorLogService _errorLog;
    // Aplicación por defecto del sistema (ajustar si cambia)
    private const string CodAplicacionDefault = "1";

    public AsignacionService(HttpClient http, ErrorLogService errorLog)
    { _http = http; _errorLog = errorLog; }

    /// <summary>Roles asignados a un usuario.</summary>
    public async Task<List<RolAsignado>> GetRolesAsync(string codUsuario)
    {
        try
        {
            var r = await _http.GetFromJsonAsync<ApiResponse<List<RolAsignado>>>(
                $"{ApiRoutes.Seguridad.AsignacionRoles}/{Uri.EscapeDataString(codUsuario)}");
            return r?.Data ?? new();
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "Asignaciones", nameof(GetRolesAsync)); return new(); }
    }

    /// <summary>Opciones asignadas a un rol + aplicación.</summary>
    public async Task<List<OpcionAsignada>> GetOpcionesAsync(string codRolUsuario, string? codAplicacion = null)
    {
        var app = codAplicacion ?? CodAplicacionDefault;
        try
        {
            var url = $"{ApiRoutes.Seguridad.AsignacionOpciones}/{Uri.EscapeDataString(codRolUsuario)}/{Uri.EscapeDataString(app)}";
            var r = await _http.GetFromJsonAsync<ApiResponse<List<OpcionAsignada>>>(url);
            return r?.Data ?? new();
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "Asignaciones", nameof(GetOpcionesAsync)); return new(); }
    }

    /// <summary>Botones de una opción (rol + aplicación + opción).</summary>
    public async Task<List<BotonAsignado>> GetBotonesAsync(string codRolUsuario, string codOpcion, string? codAplicacion = null)
    {
        var app = codAplicacion ?? CodAplicacionDefault;
        try
        {
            var url = $"{ApiRoutes.Seguridad.AsignacionBotones}/{Uri.EscapeDataString(codRolUsuario)}/{Uri.EscapeDataString(app)}/{Uri.EscapeDataString(codOpcion)}";
            var r = await _http.GetFromJsonAsync<ApiResponse<List<BotonAsignado>>>(url);
            return r?.Data ?? new();
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "Asignaciones", nameof(GetBotonesAsync)); return new(); }
    }
}
