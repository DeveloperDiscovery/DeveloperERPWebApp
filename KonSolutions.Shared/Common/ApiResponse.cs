namespace KONSolutions.Shared.Common;

public class ApiResponse<T>
{
    public bool   Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T?     Data    { get; set; }
    public int    Total   { get; set; }

    // Detalle técnico opcional del error SQL (ver ErrorSqlBox.razor) — Message sigue
    // limpio para el Snackbar/texto plano ya usado en toda la app.
    public int?    ErrorNumber    { get; set; }
    public string? ErrorProcedure { get; set; }
    public int?    ErrorLine      { get; set; }

    /// <summary>Sentencia EXEC + parámetros reconstruida por el repositorio (API) cuando
    /// atrapó una SqlException — solo viene poblada en los módulos donde ya se implementó.</summary>
    public string? SqlSentence    { get; set; }
}

public class WriteResult
{
    public int    AffectedRows { get; set; }
    public string Estado       { get; set; } = "OK";
    public string Mensaje      { get; set; } = string.Empty;
    public int?    ErrorNumber    { get; set; }
    public string? ErrorMessage   { get; set; }
    public int?    ErrorLine      { get; set; }
    public string? ErrorProcedure { get; set; }
    public string? SqlSentence    { get; set; }

    /// <summary>Clave que generó la base en el alta. Viene vacía cuando la escribe el usuario.
    /// La necesitan los documentos con detalle: sin el número de la cabecera recién creada no hay
    /// forma de insertar sus ítems.</summary>
    public string? Codigo         { get; set; }
}

public class ComboboxItem
{
    public string Id    { get; set; } = string.Empty;
    public string Texto { get; set; } = string.Empty;
    public string? Grupo { get; set; }
}

/// <summary>Item de estado con colores y flag default para EstadoCombo.</summary>
public class EstadoItem
{
    public int    CodEstado { get; set; }
    public string DesEstado { get; set; } = string.Empty;
    public string BackColor { get; set; } = string.Empty;
    public string ForeColor { get; set; } = string.Empty;
    public bool   EsDefault { get; set; }
}
