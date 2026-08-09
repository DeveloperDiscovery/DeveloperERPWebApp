using System.Net.Http.Json;
using KONSolutions.Shared.Common;
using KONSolutions.Shared.Models.Seguridad;

namespace KONSolutions.Web.Services;

public class FavoritoItem
{
    public string? COD_USUARIO { get; set; }
    public string? COD_APLICACION { get; set; }
    public string? COD_OPCION_APLICACION { get; set; }
}

/// <summary>
/// Favoritos del menú: SEGURIDAD.USUARIOS_APLICACIONES_OPCIONES_FAVORITOS.
/// Permite listar, agregar (clic derecho) y quitar opciones favoritas por aplicación.
/// </summary>
public class FavoritoService
{
    private readonly HttpClient _http;
    private readonly ErrorLogService _errorLog;
    private const string Ruta = ApiRoutes.Seguridad.Favoritos;

    public event Action? OnCambio;

    public FavoritoService(HttpClient http, ErrorLogService errorLog)
    { _http = http; _errorLog = errorLog; }

    /// <summary>Favoritos de un usuario en una aplicación.</summary>
    public async Task<List<FavoritoItem>> GetAsync(string codUsuario, string codAplicacion)
    {
        try
        {
            var r = await _http.GetFromJsonAsync<ApiResponse<List<FavoritoItem>>>(
                $"{Ruta}/{Uri.EscapeDataString(codUsuario)}/{Uri.EscapeDataString(codAplicacion)}");
            return r?.Data ?? new();
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "Favoritos", nameof(GetAsync)); return new(); }
    }

    public async Task<bool> AgregarAsync(string codUsuario, string codAplicacion, string codOpcion)
    {
        try
        {
            var dto = new { COD_USUARIO = codUsuario, COD_APLICACION = codAplicacion, COD_OPCION_APLICACION = codOpcion };
            var resp = await _http.PostAsJsonAsync(Ruta, dto);
            if (resp.IsSuccessStatusCode) OnCambio?.Invoke();
            return resp.IsSuccessStatusCode;
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "Favoritos", nameof(AgregarAsync)); return false; }
    }

    public async Task<List<FavoritoWorkspace>> GetWorkspaceAsync(string codUsuario)
    {
        try
        {
            var r = await _http.GetFromJsonAsync<ApiResponse<List<FavoritoWorkspace>>>(
                $"{ApiRoutes.Seguridad.FavoritosWorkspace}/{Uri.EscapeDataString(codUsuario)}");
            return r?.Data ?? new();
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "Favoritos", nameof(GetWorkspaceAsync)); return new(); }
    }

    /// <summary>Experimento: íconos de los favoritos del Workspace, en un endpoint aparte del
    /// texto (GetWorkspaceAsync) — se pide en paralelo, sin bloquear el primer render.</summary>
    public async Task<List<FavoritoLogoItem>> GetWorkspaceLogosAsync(string codUsuario)
    {
        try
        {
            var r = await _http.GetFromJsonAsync<ApiResponse<List<FavoritoLogoItem>>>(
                $"{ApiRoutes.Seguridad.FavoritosWorkspaceLogos}/{Uri.EscapeDataString(codUsuario)}");
            return r?.Data ?? new();
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "Favoritos", nameof(GetWorkspaceLogosAsync)); return new(); }
    }

    public async Task<bool> QuitarAsync(string codUsuario, string codAplicacion, string codOpcion)
    {
        try
        {
            var resp = await _http.DeleteAsync(
                $"{Ruta}/{Uri.EscapeDataString(codUsuario)}/{Uri.EscapeDataString(codAplicacion)}/{Uri.EscapeDataString(codOpcion)}");
            if (resp.IsSuccessStatusCode) OnCambio?.Invoke();
            return resp.IsSuccessStatusCode;
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "Favoritos", nameof(QuitarAsync)); return false; }
    }
}
