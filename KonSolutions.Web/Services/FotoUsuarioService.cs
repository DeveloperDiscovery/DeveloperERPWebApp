using System.Collections.Concurrent;

namespace KONSolutions.Web.Services;

/// <summary>
/// Cachea en memoria la foto de cada usuario (COD_USUARIO → base64) durante la
/// sesión, para que se descargue UNA sola vez por usuario y NO viaje embebida en
/// cada respuesta del polling de notificaciones (que antes traía IMG_FOTO repetida
/// por cada notificación, cada 20 s, congelando el hilo de Blazor WASM).
///
/// La foto se pide al endpoint dedicado GET /seguridad/usuarios/{cod}/foto, que
/// devuelve el binario (no JSON base64) con Cache-Control, así incluso el navegador
/// la reutiliza. Un usuario "sin foto" (204) también se cachea como null para no
/// reintentar en cada refresco.
/// </summary>
public class FotoUsuarioService
{
    private readonly HttpClient _http;

    // null como valor = "ya consultado, no tiene foto" (distinto de "aún no consultado").
    private readonly ConcurrentDictionary<string, string?> _cache =
        new(StringComparer.OrdinalIgnoreCase);

    public FotoUsuarioService(HttpClient http) => _http = http;

    /// <summary>
    /// Devuelve la foto cacheada si ya se descargó. No dispara ninguna llamada:
    /// úsalo en el render (síncrono). Para precargar, llama a <see cref="PrecargarAsync"/>.
    /// </summary>
    public string? ObtenerCacheada(string? codUsuario)
        => !string.IsNullOrWhiteSpace(codUsuario) && _cache.TryGetValue(codUsuario, out var b64) ? b64 : null;

    /// <summary>
    /// Descarga la foto (si aún no está cacheada) y la guarda. Devuelve true si tras
    /// esta llamada hay algo nuevo que renderizar (para decidir un StateHasChanged).
    /// </summary>
    public async Task<bool> PrecargarAsync(string? codUsuario)
    {
        if (string.IsNullOrWhiteSpace(codUsuario)) return false;
        if (_cache.ContainsKey(codUsuario)) return false; // ya consultado (con o sin foto)

        try
        {
            var resp = await _http.GetAsync($"api/v1/seguridad/usuarios/{Uri.EscapeDataString(codUsuario)}/foto");
            if (resp.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                // 204 = el usuario realmente no tiene foto: cachear para no reintentar.
                _cache[codUsuario] = null;
            }
            else if (resp.IsSuccessStatusCode)
            {
                var bytes = await resp.Content.ReadAsByteArrayAsync();
                _cache[codUsuario] = bytes.Length > 0 ? Convert.ToBase64String(bytes) : null;
            }
            else
            {
                // 401/403/500 (ej. token aún no listo en el arranque): NO cachear,
                // para que el próximo intento (siguiente polling/toast) sí la traiga.
                return false;
            }
        }
        catch
        {
            // No cachear el fallo: permitir reintento en el próximo polling.
            return false;
        }
        return true;
    }

    /// <summary>Precarga en paralelo las fotos de varios usuarios distintos.</summary>
    public async Task<bool> PrecargarVariosAsync(IEnumerable<string?> codsUsuario)
    {
        var pendientes = codsUsuario
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Select(c => c!.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Where(c => !_cache.ContainsKey(c))
            .ToList();

        if (pendientes.Count == 0) return false;

        var resultados = await Task.WhenAll(pendientes.Select(PrecargarAsync));
        return resultados.Any(r => r);
    }
}
