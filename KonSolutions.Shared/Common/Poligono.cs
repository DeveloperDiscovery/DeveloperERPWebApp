using System.Globalization;
using System.Text.Json;

namespace KONSolutions.Shared.Common;

/// <summary>Un vértice del polígono en coordenadas <b>normalizadas</b> al plano:
/// 0 = borde izquierdo/superior, 1 = borde derecho/inferior.</summary>
public readonly record struct PuntoPoligono(double X, double Y);

/// <summary>
/// Serialización de DES_POLIGONO (nvarchar(max) en LOGISTICA.ALMACENES y
/// LOGISTICA.ALMACENES_UBICACIONES_ALMACEN).
///
/// Formato: lista JSON de pares — <c>[[0.12,0.34],[0.51,0.34],[0.51,0.78]]</c>.
///
/// Las coordenadas van normalizadas 0..1 y NO en píxeles a propósito: así el polígono
/// sigue siendo válido si mañana se reemplaza IMG_PLANO por un escaneo de otra resolución,
/// y el mismo dato sirve para dibujar en pantalla o para calcular metros usando
/// VAL_ANCHO_METROS / VAL_LARGO_METROS de la zonificación.
/// </summary>
public static class Poligono
{
    /// <summary>Lee DES_POLIGONO. Nunca lanza: un dato corrupto o vacío devuelve lista
    /// vacía, que la UI trata como "sin polígono dibujado".</summary>
    public static List<PuntoPoligono> Parse(string? des)
    {
        if (string.IsNullOrWhiteSpace(des)) return new();
        try
        {
            var crudos = JsonSerializer.Deserialize<List<List<double>>>(des);
            if (crudos is null) return new();
            return crudos.Where(p => p.Count >= 2)
                         .Select(p => new PuntoPoligono(p[0], p[1]))
                         .ToList();
        }
        catch (JsonException)
        {
            return new();
        }
    }

    /// <summary>Escribe DES_POLIGONO. Menos de 3 vértices no es un polígono: devuelve null
    /// para que la columna quede NULL en vez de guardar una figura degenerada.</summary>
    public static string? Serializar(IEnumerable<PuntoPoligono> puntos)
    {
        var lista = puntos.ToList();
        if (lista.Count < 3) return null;
        // 5 decimales ≈ 1 cm sobre un plano de 100 m: sobra, y evita cadenas enormes.
        var pares = lista.Select(p => new[]
        {
            Math.Round(Clamp01(p.X), 5),
            Math.Round(Clamp01(p.Y), 5)
        });
        return JsonSerializer.Serialize(pares);
    }

    public static bool TienePoligono(string? des) => Parse(des).Count >= 3;

    /// <summary>Puntos en el formato del atributo <c>points</c> de un &lt;polygon&gt; SVG,
    /// escalados al viewBox que se le pase.</summary>
    public static string ASvgPoints(IEnumerable<PuntoPoligono> puntos, double ancho, double alto)
        => string.Join(" ", puntos.Select(p =>
            string.Create(CultureInfo.InvariantCulture, $"{p.X * ancho:0.##},{p.Y * alto:0.##}")));

    /// <summary>Centro aproximado (promedio de vértices) — sirve para poner la etiqueta
    /// encima de la figura. No es el centroide exacto de un polígono irregular, pero para
    /// ubicar un texto alcanza y es mucho más barato de calcular.</summary>
    public static PuntoPoligono Centro(IReadOnlyList<PuntoPoligono> puntos)
    {
        if (puntos.Count == 0) return new PuntoPoligono(0, 0);
        return new PuntoPoligono(puntos.Average(p => p.X), puntos.Average(p => p.Y));
    }

    /// <summary>Área en metros cuadrados, por la fórmula del cordón (shoelace). Requiere las
    /// medidas reales del nivel: sin ellas el área normalizada no significa nada.</summary>
    public static double AreaM2(IReadOnlyList<PuntoPoligono> puntos, int? anchoMetros, int? largoMetros)
    {
        if (puntos.Count < 3 || anchoMetros is not > 0 || largoMetros is not > 0) return 0;
        double suma = 0;
        for (var i = 0; i < puntos.Count; i++)
        {
            var a = puntos[i];
            var b = puntos[(i + 1) % puntos.Count];
            suma += (a.X * b.Y) - (b.X * a.Y);
        }
        return Math.Abs(suma) / 2d * anchoMetros.Value * largoMetros.Value;
    }

    /// <summary>¿El punto cae dentro del polígono? Lanzado de rayos: se cuenta cuántas
    /// aristas cruza una semirrecta horizontal hacia la derecha; impar = adentro.
    /// Funciona con figuras cóncavas, que es el caso normal de un almacén en forma de L.</summary>
    public static bool Contiene(IReadOnlyList<PuntoPoligono> poligono, PuntoPoligono p)
    {
        if (poligono.Count < 3) return false;
        var dentro = false;
        for (int i = 0, j = poligono.Count - 1; i < poligono.Count; j = i++)
        {
            var a = poligono[i];
            var b = poligono[j];
            if (a.Y > p.Y != b.Y > p.Y &&
                p.X < (b.X - a.X) * (p.Y - a.Y) / (b.Y - a.Y) + a.X)
                dentro = !dentro;
        }
        return dentro;
    }

    /// <summary>¿La figura entra COMPLETA dentro del polígono?
    ///
    /// No alcanza con que sus vértices estén adentro: en un almacén cóncavo —una L, un
    /// almacén con una columna en el medio— un rack puede tener las cuatro esquinas dentro
    /// y aun así cruzar por fuera con uno de sus lados. Por eso además se verifica que
    /// ninguna arista de la figura corte una arista del polígono.</summary>
    public static bool ContieneFigura(IReadOnlyList<PuntoPoligono> poligono, IReadOnlyList<PuntoPoligono> figura)
    {
        if (poligono.Count < 3 || figura.Count < 3) return false;
        if (figura.Any(v => !Contiene(poligono, v))) return false;

        for (var i = 0; i < figura.Count; i++)
        {
            var f1 = figura[i];
            var f2 = figura[(i + 1) % figura.Count];
            for (var j = 0; j < poligono.Count; j++)
            {
                var p1 = poligono[j];
                var p2 = poligono[(j + 1) % poligono.Count];
                if (SeCruzan(f1, f2, p1, p2)) return false;
            }
        }
        return true;
    }

    /// <summary>¿Se superponen dos figuras convexas? Se pisan si alguna arista corta a otra,
    /// o si una está contenida entera en la otra (caso en el que ninguna arista se cruza).
    ///
    /// Apoyarse no es superponerse: dos racks pegados comparten la pared y sus aristas son
    /// colineales, algo que <see cref="SeCruzan"/> deja pasar a propósito. Aun así, quien
    /// llame conviene que encoja un par de centímetros las figuras antes de comparar, para
    /// que el redondeo de las coordenadas no convierta un contacto en un solapamiento.</summary>
    public static bool SeSuperponen(IReadOnlyList<PuntoPoligono> a, IReadOnlyList<PuntoPoligono> b)
    {
        if (a.Count < 3 || b.Count < 3) return false;

        for (var i = 0; i < a.Count; i++)
        {
            var a1 = a[i];
            var a2 = a[(i + 1) % a.Count];
            for (var j = 0; j < b.Count; j++)
                if (SeCruzan(a1, a2, b[j], b[(j + 1) % b.Count])) return true;
        }

        // Sin cruces, o son ajenas o una está adentro de la otra: alcanza con probar un
        // vértice de cada una.
        return Contiene(b, a[0]) || Contiene(a, b[0]);
    }

    /// <summary>¿Se cruzan los segmentos a1-a2 y b1-b2? Por orientación de los extremos.
    /// El caso colineal se deja pasar como "no cruzan": un rack apoyado exactamente contra
    /// la pared del almacén está adentro, no afuera.</summary>
    private static bool SeCruzan(PuntoPoligono a1, PuntoPoligono a2, PuntoPoligono b1, PuntoPoligono b2)
    {
        var d1 = Orientacion(b1, b2, a1);
        var d2 = Orientacion(b1, b2, a2);
        var d3 = Orientacion(a1, a2, b1);
        var d4 = Orientacion(a1, a2, b2);
        return d1 * d2 < 0 && d3 * d4 < 0;
    }

    /// <summary>Signo del producto cruzado: positivo, negativo o cero según de qué lado de
    /// la recta p-q cae r. La tolerancia evita que el redondeo a 5 decimales convierta un
    /// vértice apoyado sobre la pared en un cruce.</summary>
    private static double Orientacion(PuntoPoligono p, PuntoPoligono q, PuntoPoligono r)
    {
        var v = (q.X - p.X) * (r.Y - p.Y) - (q.Y - p.Y) * (r.X - p.X);
        return Math.Abs(v) < 1e-9 ? 0 : Math.Sign(v);
    }

    private static double Clamp01(double v) => v < 0 ? 0 : v > 1 ? 1 : v;
}
