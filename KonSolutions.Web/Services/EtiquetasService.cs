using System.Net.Http.Json;
using KONSolutions.Shared.Common;
using KONSolutions.Shared.Models.Entorno;

namespace KONSolutions.Web.Services;

/// <summary>
/// Diccionario ENTORNO.DICCIONARIO_ETIQUETAS (COD_ETIQUETA = nombre de campo de BD,
/// ej. "DES_PRODUCTO" -> "Descripción de producto") — se carga UNA VEZ al iniciar sesión
/// (MainLayout.OnInitializedAsync, junto a ConfigSvc/MenuService) y queda en memoria.
///
/// Se usa como VALOR POR DEFECTO de labels/encabezados: los componentes que ya reciben un
/// Label/Texto explícito (los ~74 diálogos y grillas existentes) siguen mostrando ese texto
/// sin cambios — el diccionario solo entra a jugar cuando el llamador NO pasa un texto
/// explícito, dejando que CampoTexto/EncabezadoOrdenable lo resuelvan solos por el nombre
/// de columna/campo.
/// </summary>
public class EtiquetasService
{
    private readonly HttpClient _http;
    private readonly ErrorLogService _errorLog;
    private Dictionary<string, string> _etiquetas = new(StringComparer.OrdinalIgnoreCase);
    private bool _cargado;

    public EtiquetasService(HttpClient http, ErrorLogService errorLog)
    { _http = http; _errorLog = errorLog; }

    /// <summary>Carga el diccionario desde la API. Por defecto solo la primera vez
    /// (<paramref name="forzar"/> = false) — pasar true para recargar aunque ya esté
    /// cargado (se usa al editar ENTORNO.DICCIONARIO_ETIQUETAS, ver
    /// DiccionarioEtiquetasDialog.Submit y MainLayout.RefrescarEtiquetasPorCambio).</summary>
    public async Task CargarAsync(bool forzar = false)
    {
        if (_cargado && !forzar) return;
        try
        {
            var r = await _http.GetFromJsonAsync<ApiResponse<List<DiccionarioEtiqueta>>>(ApiRoutes.Entorno.DiccionarioEtiquetas);
            _etiquetas = (r?.Data ?? new())
                .Where(e => !string.IsNullOrWhiteSpace(e.COD_ETIQUETA))
                .ToDictionary(e => e.COD_ETIQUETA, e => e.DES_ETIQUETA ?? e.COD_ETIQUETA, StringComparer.OrdinalIgnoreCase);
            _cargado = true;
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "EtiquetasService", nameof(CargarAsync)); }
    }

    /// <summary>Etiqueta para <paramref name="codEtiqueta"/> (nombre de campo/columna).
    /// Si no está en el diccionario, devuelve <paramref name="valorPorDefecto"/> (o el propio
    /// código si no se pasó nada) — nunca deja el control sin texto.</summary>
    public string Obtener(string codEtiqueta, string? valorPorDefecto = null)
    {
        if (string.IsNullOrWhiteSpace(codEtiqueta)) return valorPorDefecto ?? "";
        return _etiquetas.TryGetValue(codEtiqueta, out var etiqueta) ? etiqueta : (valorPorDefecto ?? codEtiqueta);
    }
}
