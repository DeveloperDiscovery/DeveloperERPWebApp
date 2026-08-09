using System.Net.Http.Json;
using KONSolutions.Shared.Common;
using KONSolutions.Shared.Models.Entorno;

namespace KONSolutions.Web.Services;

/// <summary>Ficha completa de una notificación de aprobación (cabecera + documentos + ejecución),
/// para el diálogo que se abre al hacer clic en una notificación del AppBar.</summary>
public class NotificacionesAprobacionesService
{
    private readonly HttpClient _http;
    public NotificacionesAprobacionesService(HttpClient http) => _http = http;

    public async Task<NotificacionAprobacionResult?> GetByIdAsync(int numNotificacion)
    {
        try
        {
            var resp = await _http.GetFromJsonAsync<ApiResponse<NotificacionAprobacionResult>>(
                $"{ApiRoutes.Entorno.NotificacionesAprobaciones}/{numNotificacion}");
            return resp?.Data;
        }
        catch { return null; }
    }
}
