namespace KONSolutions.Shared.Common;

/// <summary>Un contenedor sobre el plano: rectángulo orientado, en METROS reales del nivel.
///
/// Por qué rectángulo y no polígono libre: los casilleros son una matriz de filas ×
/// columnas. Una figura irregular no se puede subdividir en esa matriz sin inventar reglas,
/// así que el polígono libre haría imposible derivar la posición de cada casillero — que es
/// justamente lo que evita tener que dibujar ubicación por ubicación.
///
/// Por qué en metros y no en las coordenadas normalizadas 0–1 del plano: la normalización
/// divide X por el ancho del nivel e Y por el largo, así que un nivel que no es cuadrado
/// tiene escalas distintas por eje. Girar en ese espacio deformaría el rectángulo (un giro
/// de 90° cambiaría sus medidas). En metros el giro es un giro.
///
/// CONVENCIÓN: el vértice <see cref="Origen"/> es la esquina de FILA 1 · COLUMNA 1, y las
/// columnas avanzan hacia <see cref="Angulo"/>. De ahí sale también el lado de acceso.
/// Los cuatro vértices se guardan en ese orden en DES_POLIGONO, con el formato normalizado
/// de siempre: no cambia ninguna tabla.</summary>
public readonly record struct RectanguloContenedor(
    double X, double Y, double Ancho, double Fondo, double Angulo)
{
    /// <summary>Esquina de fila 1 · columna 1, en metros.</summary>
    public (double X, double Y) Origen => (X, Y);

    /// <summary>Dirección en la que avanzan las columnas (vector unitario).</summary>
    public (double X, double Y) EjeColumnas => (Math.Cos(Angulo), Math.Sin(Angulo));

    /// <summary>Dirección del fondo, perpendicular a las columnas (vector unitario).</summary>
    public (double X, double Y) EjeFondo => (-Math.Sin(Angulo), Math.Cos(Angulo));

    public (double X, double Y) Centro
        => (X + (EjeColumnas.X * Ancho + EjeFondo.X * Fondo) / 2,
            Y + (EjeColumnas.Y * Ancho + EjeFondo.Y * Fondo) / 2);

    public double AreaM2 => Math.Abs(Ancho * Fondo);

    /// <summary>Los cuatro vértices en metros, en el orden de la convención:
    /// origen → avance de columnas → fondo → vuelta.</summary>
    public IReadOnlyList<(double X, double Y)> VerticesM()
    {
        var (ux, uy) = EjeColumnas;
        var (vx, vy) = EjeFondo;
        return new[]
        {
            (X, Y),
            (X + ux * Ancho, Y + uy * Ancho),
            (X + ux * Ancho + vx * Fondo, Y + uy * Ancho + vy * Fondo),
            (X + vx * Fondo, Y + vy * Fondo)
        };
    }

    /// <summary>Huella de una columna concreta (1..columnas). Las filas se apilan en altura
    /// sobre esta misma huella, así que en vista de planta todas comparten la franja.</summary>
    public IReadOnlyList<(double X, double Y)> ColumnaM(int columna, int columnas)
    {
        if (columnas < 1) columnas = 1;
        var k = Math.Clamp(columna, 1, columnas);
        var (ux, uy) = EjeColumnas;
        var (vx, vy) = EjeFondo;
        var a = Ancho * (k - 1) / columnas;
        var b = Ancho * k / columnas;
        return new[]
        {
            (X + ux * a,             Y + uy * a),
            (X + ux * b,             Y + uy * b),
            (X + ux * b + vx * Fondo, Y + uy * b + vy * Fondo),
            (X + ux * a + vx * Fondo, Y + uy * a + vy * Fondo)
        };
    }

    /// <summary>Construye el rectángulo desde dos esquinas opuestas sin girar. Es el gesto de
    /// dibujo: se arrastra de una esquina a la otra.</summary>
    public static RectanguloContenedor DesdeArrastre(double x0, double y0, double x1, double y1)
        => new RectanguloContenedor(Math.Min(x0, x1), Math.Min(y0, y1),
               Math.Abs(x1 - x0), Math.Abs(y1 - y0), 0).ConColumnasALoLargo();

    /// <summary>Deja el eje de columnas sobre el lado LARGO del mueble.
    ///
    /// Las columnas de un rack corren a lo largo; las filas se apilan en altura. Si el eje de
    /// columnas cayera en el lado corto —cosa que pasa según hacia dónde se arrastre al
    /// dibujar— las divisiones saldrían sobre la cara angosta y la numeración de los
    /// casilleros no tendría nada que ver con el mueble real.
    ///
    /// Gira el marco 90° tomando como nuevo origen el vértice contiguo: la huella sobre el
    /// plano queda EXACTAMENTE igual, sólo cambia cuál de los dos ejes cuenta columnas. Como
    /// efecto, la esquina de fila 1 · columna 1 se corre al vértice vecino — que es lo
    /// correcto: la columna 1 tiene que arrancar en una punta del lado largo.</summary>
    public RectanguloContenedor ConColumnasALoLargo()
    {
        if (Fondo <= Ancho) return this;
        var (ux, uy) = EjeColumnas;
        return new RectanguloContenedor(
            X + ux * Ancho, Y + uy * Ancho,   // vértice contiguo: nuevo origen
            Fondo, Ancho,                     // los ejes se intercambian
            Angulo + Math.PI / 2);
    }

    /// <summary>Lee el rectángulo desde el DES_POLIGONO normalizado. Devuelve null si lo
    /// guardado no son cuatro vértices — por ejemplo un polígono libre dibujado con el editor
    /// anterior: en ese caso conviene avisar y que se redibuje, no adivinar.</summary>
    public static RectanguloContenedor? Desde(string? des, double anchoMetros, double largoMetros)
    {
        var pts = Poligono.Parse(des);
        if (pts.Count != 4 || anchoMetros <= 0 || largoMetros <= 0) return null;

        var m = pts.Select(p => (X: p.X * anchoMetros, Y: p.Y * largoMetros)).ToList();
        var ancho = Distancia(m[0], m[1]);
        var fondo = Distancia(m[0], m[3]);
        if (ancho <= 0 || fondo <= 0) return null;

        var ang = Math.Atan2(m[1].Y - m[0].Y, m[1].X - m[0].X);
        // También al leer: un contenedor guardado antes de esta corrección puede tener el eje
        // de columnas sobre el lado corto. Se endereza al mostrarlo y queda firme al guardar.
        return new RectanguloContenedor(m[0].X, m[0].Y, ancho, fondo, ang).ConColumnasALoLargo();
    }

    /// <summary>Serializa al mismo formato normalizado 0–1 que usa todo el plano, de modo que
    /// el rectángulo sobreviva a un cambio de resolución de la imagen.</summary>
    public string? Serializar(double anchoMetros, double largoMetros)
    {
        if (anchoMetros <= 0 || largoMetros <= 0) return null;
        return Poligono.Serializar(
            VerticesM().Select(v => new PuntoPoligono(v.X / anchoMetros, v.Y / largoMetros)));
    }

    /// <summary>El mismo rectángulo encogido unos centímetros por cada lado. Sirve para
    /// comparar contra otros muebles: dos racks pegados espalda con espalda comparten la
    /// pared, y sin este margen el redondeo de las coordenadas haría que ese contacto se
    /// leyera como una superposición. En muebles muy chicos el margen se reduce para no
    /// dejarlos en nada.</summary>
    public RectanguloContenedor Encogido(double margen)
    {
        var m = Math.Min(margen, Math.Min(Ancho, Fondo) / 4);
        if (m <= 0) return this;
        var (ux, uy) = EjeColumnas;
        var (vx, vy) = EjeFondo;
        return new RectanguloContenedor(
            X + ux * m + vx * m,
            Y + uy * m + vy * m,
            Ancho - 2 * m, Fondo - 2 * m, Angulo);
    }

    /// <summary>Mantiene el rectángulo dentro del nivel. Se mueve entero en vez de recortarse:
    /// recortar cambiaría sus medidas, y las medidas del mueble son un dato real, no algo que
    /// el editor pueda decidir.</summary>
    public RectanguloContenedor DentroDe(double anchoMetros, double largoMetros)
    {
        var xs = VerticesM().Select(v => v.X).ToList();
        var ys = VerticesM().Select(v => v.Y).ToList();
        var dx = 0d;
        var dy = 0d;
        if (xs.Min() < 0) dx = -xs.Min();
        else if (xs.Max() > anchoMetros) dx = anchoMetros - xs.Max();
        if (ys.Min() < 0) dy = -ys.Min();
        else if (ys.Max() > largoMetros) dy = largoMetros - ys.Max();
        return dx == 0 && dy == 0 ? this : this with { X = X + dx, Y = Y + dy };
    }

    private static double Distancia((double X, double Y) a, (double X, double Y) b)
        => Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
}
