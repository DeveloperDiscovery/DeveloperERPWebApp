using System.Net.Http.Json;
using KONSolutions.Shared.Common;
using KONSolutions.Shared.Models.Estandar;

namespace KONSolutions.Web.Services;

public class ConfiguracionUsuarioService
{
    private readonly HttpClient _http;
    private readonly ErrorLogService _errorLog;

    public ConfiguracionUsuarioService(HttpClient http, ErrorLogService errorLog)
    { _http = http; _errorLog = errorLog; }

    /// <summary>Configuración del usuario actualmente autenticado.</summary>
    public async Task<ConfiguracionUsuario?> GetMiConfiguracionAsync()
    {
        try
        {
            var r = await _http.GetFromJsonAsync<ApiResponse<ConfiguracionUsuario>>(ApiRoutes.Estandar.ConfiguracionUsuarioMia);
            return r?.Data;
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "ConfiguracionUsuario", nameof(GetMiConfiguracionAsync)); return null; }
    }

    public async Task<(bool ok, string msg)> SaveAsync(ConfiguracionUsuario cfg)
    {
        try
        {
            var resp = await _http.PostAsJsonAsync(ApiRoutes.Estandar.ConfiguracionUsuario, cfg);
            if (resp.IsSuccessStatusCode) return (true, "Configuración guardada.");
            var msg = await resp.Content.ReadAsStringAsync();
            return (false, msg);
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "ConfiguracionUsuario", nameof(SaveAsync)); return (false, ex.Message); }
    }
}
