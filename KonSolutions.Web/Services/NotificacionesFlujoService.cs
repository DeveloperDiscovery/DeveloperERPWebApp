using System.Net.Http.Json;
using KONSolutions.Shared.Common;
using KONSolutions.Shared.Models.Entorno;

namespace KONSolutions.Web.Services;

/// <summary>
/// Notificaciones pendientes del usuario logueado, para el grupo de íconos del AppBar
/// (uno por COD_TIPO_NOTIFICACION, con badge de conteo). Se refresca periódicamente
/// desde MainLayout junto con el resto de indicadores del entorno.
/// </summary>
public class NotificacionesFlujoService
{
    private readonly HttpClient _http;
    public NotificacionesFlujoService(HttpClient http) => _http = http;

    public async Task<NotificacionesFlujoResult> GetPendientesAsync()
    {
        try
        {
            var resp = await _http.GetFromJsonAsync<ApiResponse<NotificacionesFlujoResult>>(
                ApiRoutes.Entorno.NotificacionesFlujoPendientes);
            return resp?.Data ?? new NotificacionesFlujoResult();
        }
        catch { return new NotificacionesFlujoResult(); }
    }

    /// <summary>Listado del módulo de consulta. recepcionada=true → Recibidas; false → Emitidas.
    /// codUsuario opcional (por defecto, el del token).</summary>
    public async Task<List<NotificacionFlujoDetalle>> GetShowallAsync(bool recepcionada, string? codUsuario = null)
    {
        try
        {
            var url = $"{ApiRoutes.Entorno.NotificacionesFlujoShowall}?recepcionada={recepcionada.ToString().ToLower()}";
            if (!string.IsNullOrWhiteSpace(codUsuario)) url += $"&codUsuario={Uri.EscapeDataString(codUsuario)}";
            var resp = await _http.GetFromJsonAsync<ApiResponse<List<NotificacionFlujoDetalle>>>(url);
            return resp?.Data ?? new();
        }
        catch { return new(); }
    }
}
