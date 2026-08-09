using System.Net.Http.Json;
using KONSolutions.Shared.Common;

namespace KONSolutions.Web.Services;

// Extraído de los ~56 dialogs de CRUD estándar, cada uno tenía una copia privada
// idéntica de este método (SafeGetAsync<T>) para recargar el registro fresco desde
// el endpoint ShowbyId tras abrir el dialog.
public static class HttpClientCrudExtensions
{
    public static async Task<T?> SafeGetAsync<T>(this HttpClient http, string url) where T : class
    {
        try
        {
            var r = await http.GetFromJsonAsync<ApiResponse<T>>(url);
            return r?.Data;
        }
        catch { return null; }
    }
}
