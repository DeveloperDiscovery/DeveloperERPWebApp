using System.Net.Http.Json;

namespace KONSolutions.Web.Services;

/// <summary>Guardado de un registro eligiendo el verbo según la intención.
///
/// Desde que los SP dejaron de hacer upsert, el verbo dejó de ser un detalle: un POST sobre
/// una clave que ya existe es rechazado, y un PUT sobre una que no existe también. La pantalla
/// es la única que sabe cuál de las dos cosas está haciendo, porque abrió el diálogo en modo
/// alta o en modo edición.
///
/// Se centraliza acá en vez de repetir el ternario en cada diálogo: son casi cien pantallas, y
/// el día que haya que cambiar algo —un encabezado, un reintento, una traza— conviene que sea
/// un solo lugar y no noventa.</summary>
public static class HttpGuardarExtensions
{
    /// <param name="esNuevo">true = alta (POST) · false = modificación (PUT).</param>
    public static Task<HttpResponseMessage> GuardarAsync<T>(
        this HttpClient http, string ruta, T dto, bool esNuevo)
        => esNuevo
            ? http.PostAsJsonAsync(ruta, dto)
            : http.PutAsJsonAsync(ruta, dto);
}
