using System.Text.Json;

namespace KONSolutions.Web.Services;

/// <summary>
/// Extrae un mensaje legible del cuerpo de una respuesta HTTP fallida.
/// Cubre dos formatos que puede devolver la API:
///   1) ApiResponse&lt;T&gt; con Message ya seteado (caso normal, error de SP/negocio).
///   2) SerializableError de ModelState (BadRequest(ModelState) sin envolver en ApiResponse),
///      p.ej. {"COD_CENTRO_COSTO_PADRE":["El campo COD_CENTRO_COSTO_PADRE es requerido."]} —
///      antes esto caía silenciosamente en "Operación rechazada." porque no calzaba con
///      la forma de ApiResponse y el mensaje real de validación se perdía.
/// </summary>
public static class ApiErrorHelper
{
    public static string ExtraerMensaje(string raw, string mensajeApiResponse, int httpStatus)
    {
        if (!string.IsNullOrWhiteSpace(mensajeApiResponse)) return mensajeApiResponse;

        if (!string.IsNullOrWhiteSpace(raw))
        {
            try
            {
                using var doc = JsonDocument.Parse(raw);
                if (doc.RootElement.ValueKind == JsonValueKind.Object)
                {
                    // Si viene "errors" (ValidationProblemDetails de [ApiController]), usar solo eso;
                    // si no, caer al SerializableError plano de BadRequest(ModelState).
                    var partes = new List<string>();
                    if (doc.RootElement.TryGetProperty("errors", out var errores))
                        RecolectarMensajes(errores, partes);
                    else
                        RecolectarMensajes(doc.RootElement, partes);
                    if (partes.Count > 0) return string.Join(" ", partes.Distinct());
                }
            }
            catch (JsonException) { /* no era JSON de ModelState; se ignora */ }
        }

        return $"Operación rechazada (HTTP {httpStatus}).";
    }

    /// <summary>
    /// Recorre el JSON buscando mensajes de error en cualquier nivel: cubre tanto el
    /// SerializableError plano (BadRequest(ModelState) manual: {"campo":["msg"]}) como el
    /// ValidationProblemDetails que [ApiController] genera automáticamente
    /// ({"title":"...","errors":{"campo":["msg"]},"status":400,...}), donde los mensajes
    /// quedan anidados dentro de la propiedad "errors" en vez de en la raíz.
    /// </summary>
    private static void RecolectarMensajes(JsonElement el, List<string> partes)
    {
        switch (el.ValueKind)
        {
            case JsonValueKind.Object:
                foreach (var prop in el.EnumerateObject())
                {
                    // "type"/"title"/"status"/"traceId" son metadatos de ValidationProblemDetails, no errores de campo.
                    if (prop.Name is "type" or "traceId") continue;
                    RecolectarMensajes(prop.Value, partes);
                }
                break;
            case JsonValueKind.Array:
                foreach (var item in el.EnumerateArray()) RecolectarMensajes(item, partes);
                break;
            case JsonValueKind.String:
                var s = el.GetString();
                if (!string.IsNullOrWhiteSpace(s)) partes.Add(s!);
                break;
        }
    }
}
