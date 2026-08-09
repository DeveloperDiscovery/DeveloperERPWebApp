namespace KONSolutions.Shared.Models.Grid;

/// <summary>
/// Tipo de datos a mostrar en la celda de la grilla dinámica.
/// </summary>
public enum GridColumnType
{
    /// <summary>Se infiere del tipo .NET de la propiedad.</summary>
    Auto,
    Text,
    Number,
    Date,
    Boolean,
    Image
}

/// <summary>
/// Descriptor de columna para DynamicMudGrid. Define qué campo mostrar, cómo
/// mostrarlo y si se permite filtrar/ordenar. No depende de T para poder
/// compartirse fácilmente entre vistas.
/// </summary>
public sealed class GridColumnDef
{
    /// <summary>Nombre de la propiedad del modelo (vía reflection).</summary>
    public required string Field { get; init; }

    /// <summary>Título visible en la cabecera.</summary>
    public required string Title { get; init; }

    public bool            Sortable   { get; init; } = true;
    public bool            Filterable { get; init; } = true;
    public bool            Visible    { get; init; } = true;

    /// <summary>Ancho CSS opcional, p. ej. "120px" o "auto".</summary>
    public string?         Width      { get; init; }

    public GridColumnType  ColumnType { get; init; } = GridColumnType.Auto;

    /// <summary>
    /// Formato de presentación para Number y Date, p. ej. "N2", "dd/MM/yyyy".
    /// </summary>
    public string?         Format     { get; init; }

    // ─── Factories de conveniencia ──────────────────────────────────────────

    public static GridColumnDef Text(string field, string title,
        bool filterable = true, bool sortable = true, string? width = null)
        => new() { Field = field, Title = title,
                   ColumnType = GridColumnType.Text,
                   Filterable = filterable, Sortable = sortable, Width = width };

    public static GridColumnDef Number(string field, string title,
        string? format = null, bool filterable = true)
        => new() { Field = field, Title = title,
                   ColumnType = GridColumnType.Number,
                   Format = format, Filterable = filterable };

    public static GridColumnDef Date(string field, string title,
        string format = "dd/MM/yyyy", bool filterable = true)
        => new() { Field = field, Title = title,
                   ColumnType = GridColumnType.Date,
                   Format = format, Filterable = filterable };

    public static GridColumnDef Bool(string field, string title)
        => new() { Field = field, Title = title,
                   ColumnType = GridColumnType.Boolean,
                   Sortable = false, Filterable = false };

    public static GridColumnDef Image(string field, string title, string? width = "60px")
        => new() { Field = field, Title = title,
                   ColumnType = GridColumnType.Image,
                   Sortable = false, Filterable = false, Width = width };
}
