namespace KONSolutions.Shared.Models.Entorno;

/// <summary>
/// Metadatos de tipo/tamaño de una columna de la BD.
/// Devueltos por el endpoint /entorno/limites-columnas (que ejecuta
/// PROC_ENTORNO_DATABASE_TIPOS_DATO_SWHOWALL).
/// </summary>
public class LimiteColumna
{
    public string TABLE_SCHEMA { get; set; } = "";
    public string TABLE_NAME { get; set; } = "";
    public string COLUMN_NAME { get; set; } = "";
    public string DATA_TYPE { get; set; } = "";
    public string IS_NULLABLE { get; set; } = "YES";   // "NO" = columna obligatoria (NOT NULL)
    public int? CHARACTER_MAXIMUM_LENGTH { get; set; }
    public int? NUMERIC_PRECISION { get; set; }
    public int? NUMERIC_SCALE { get; set; }
}

/// <summary>Catálogo ENTORNO.TIPOS_NOTIFICACION — tipos de notificación del sistema.</summary>
public class TipoNotificacion
{
    public string  COD_TIPO_NOTIFICACION { get; set; } = "";
    public string? DES_TIPO_NOTIFICACION { get; set; }
    public bool?   FLG_APROBACION        { get; set; }
    public bool?   FLG_CALENDARIO        { get; set; }
    public bool?   FLG_MENSAJE           { get; set; }
    public short?  COD_TIPO_ESTADO       { get; set; }
    public byte[]? IMG_ICONO             { get; set; }
    public string? DES_TIPO_ESTADO       { get; set; }
}

public class TipoNotificacionDto
{
    public string  COD_TIPO_NOTIFICACION { get; set; } = "";
    public string? DES_TIPO_NOTIFICACION { get; set; }
    public bool?   FLG_APROBACION        { get; set; }
    public bool?   FLG_CALENDARIO        { get; set; }
    public bool?   FLG_MENSAJE           { get; set; }
    public short?  COD_TIPO_ESTADO       { get; set; }
    public byte[]? IMG_ICONO             { get; set; }
}

/// <summary>Catálogo ENTORNO.BANCO_COLORES — banco de colores reutilizables en toda la app.</summary>
public class BancoColor
{
    public int    COD_BANCO_COLOR  { get; set; }
    public string DES_BANCO_COLOR  { get; set; } = "";
    public string DES_CODIGO_COLOR { get; set; } = "#FFFFFF";
}

/// <summary>COD_BANCO_COLOR = 0 en un alta nueva (la BD lo asigna vía IDENTITY).</summary>
public class BancoColorDto
{
    public int    COD_BANCO_COLOR  { get; set; }
    public string DES_BANCO_COLOR  { get; set; } = "";
    public string DES_CODIGO_COLOR { get; set; } = "#FFFFFF";
}
