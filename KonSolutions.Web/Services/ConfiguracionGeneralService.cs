using System.Net.Http.Json;
using KONSolutions.Shared.Common;
using KONSolutions.Shared.Models.Estandar;

namespace KONSolutions.Web.Services;

public class ConfiguracionGeneralService
{
    private readonly HttpClient _http;
    private readonly ErrorLogService _errorLog;
    private const string Ruta = "api/v1/estandar/configuracion-general";

    public ConfiguracionGeneralService(HttpClient http, ErrorLogService errorLog)
    { _http = http; _errorLog = errorLog; }

    public async Task<ConfiguracionGeneral?> GetCurrentAsync()
    {
        try
        {
            var r = await _http.GetFromJsonAsync<ApiResponse<List<ConfiguracionGeneral>>>(Ruta);
            return r?.Data?.FirstOrDefault();
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "ConfiguracionGeneral", nameof(GetCurrentAsync)); return null; }
    }

    /// <summary>Para la pantalla de Login (sin sesión todavía) — GetAll()/GetCurrentAsync
    /// ahora exige autenticación (traía datos fiscales/legales expuestos sin token). Este
    /// endpoint devuelve solo lo cosmético (logo/colores/tipografía) y sigue anónimo.</summary>
    public async Task<ConfiguracionGeneralPublica?> GetPublicAsync()
    {
        try
        {
            var r = await _http.GetFromJsonAsync<ApiResponse<ConfiguracionGeneralPublica>>($"{Ruta}/public");
            return r?.Data;
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "ConfiguracionGeneral", nameof(GetPublicAsync)); return null; }
    }

    /// <summary>Devuelve la respuesta cruda para que el llamador pueda mostrar el error
    /// real (ErrorDialogHelper.MostrarAsync) en vez de un mensaje genérico.</summary>
    public Task<HttpResponseMessage> SaveRawAsync(ConfiguracionGeneral cfg) => _http.PostAsJsonAsync(Ruta, cfg);

    public async Task<bool> SaveAsync(ConfiguracionGeneral cfg)
    {
        try
        {
            var resp = await SaveRawAsync(cfg);
            return resp.IsSuccessStatusCode;
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "ConfiguracionGeneral", nameof(SaveAsync)); return false; }
    }
}
