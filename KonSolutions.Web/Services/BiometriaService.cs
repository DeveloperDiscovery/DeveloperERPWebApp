using System.Net.Http.Json;
using KONSolutions.Shared.Common;
using KONSolutions.Shared.Models.Biometria;

namespace KONSolutions.Web.Services;

public class BiometriaService
{
    private readonly HttpClient _http;
    private readonly ErrorLogService _errorLog;

    public BiometriaService(HttpClient http, ErrorLogService errorLog)
    { _http = http; _errorLog = errorLog; }

    public async Task<List<BiometriaEstadoDto>> GetEstadosAsync()
    {
        try
        {
            var r = await _http.GetFromJsonAsync<ApiResponse<List<BiometriaEstadoDto>>>(ApiRoutes.Biometria.Estados);
            return r?.Data ?? new();
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "BIOMETRIA", nameof(GetEstadosAsync)); return new(); }
    }

    public async Task<BiometriaEstadoDto?> GetEstadoAsync(string codUsuario)
    {
        try
        {
            var r = await _http.GetFromJsonAsync<ApiResponse<BiometriaEstadoSimpleDto>>($"{ApiRoutes.Biometria.Estado}/{codUsuario}");
            if (r?.Data is null) return null;
            return new BiometriaEstadoDto
            {
                COD_USUARIO   = codUsuario,
                FLG_REGISTRADO = r.Data.FLG_REGISTRADO,
                NUM_MUESTRAS   = r.Data.NUM_MUESTRAS,
                FEC_REGISTRO   = r.Data.FEC_REGISTRO
            };
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "BIOMETRIA", nameof(GetEstadoAsync)); return null; }
    }

    private class BiometriaEstadoSimpleDto
    {
        public bool      FLG_REGISTRADO { get; set; }
        public int       NUM_MUESTRAS   { get; set; }
        public DateTime? FEC_REGISTRO   { get; set; }
    }

    public async Task<(bool ok, string msg)> RegistrarAsync(string codUsuario, List<double[]> descriptores)
    {
        try
        {
            var resp = await _http.PostAsJsonAsync(ApiRoutes.Biometria.Registrar,
                new BiometriaRegistrarRequest { COD_USUARIO = codUsuario, Descriptores = descriptores });
            var r = await resp.Content.ReadFromJsonAsync<ApiResponse<string>>();
            return (resp.IsSuccessStatusCode && r?.Success == true, r?.Message ?? "Error");
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "BIOMETRIA", nameof(RegistrarAsync)); return (false, ex.Message); }
    }

    public async Task<BiometriaResultadoDto?> VerificarAsync(string codUsuario, double[] descriptor)
    {
        try
        {
            var resp = await _http.PostAsJsonAsync(ApiRoutes.Biometria.Verificar,
                new BiometriaVerificarRequest { COD_USUARIO = codUsuario, Descriptor = descriptor });
            var r = await resp.Content.ReadFromJsonAsync<ApiResponse<BiometriaResultadoDto>>();
            return r?.Data;
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "BIOMETRIA", nameof(VerificarAsync)); return null; }
    }

    public async Task<(bool ok, string msg)> EliminarAsync(string codUsuario)
    {
        try
        {
            var resp = await _http.DeleteAsync($"{ApiRoutes.Biometria.Base}/{codUsuario}");
            var r = await resp.Content.ReadFromJsonAsync<ApiResponse<string>>();
            return (resp.IsSuccessStatusCode, r?.Message ?? "Error");
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "BIOMETRIA", nameof(EliminarAsync)); return (false, ex.Message); }
    }
}
