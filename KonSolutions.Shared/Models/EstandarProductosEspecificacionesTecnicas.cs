namespace KONSolutions.Shared.Models.Estandar;

/// <summary>Entidad ESTANDAR.PRODUCTOS_ESPECIFICACIONES_TECNICAS - valores de las
/// Especificaciones Técnicas asignadas a un Producto puntual.</summary>
public class ProductosEspecificacionesTecnicas
{
    public string COD_PRODUCTO { get; set; } = "";
    public string COD_ESPECIFICACION_TECNICA { get; set; } = "";
    public string? DES_ESPECIFICACION_TECNICA { get; set; }
    public string? DES_ESPECIFICACION_TECNICA_SINGULAR { get; set; }
    public string? COD_ESPECIFICACION_TECNICA_PADRE { get; set; }
    public int? COD_ITEM_TECNICO { get; set; }
    public DateTime? FEC_ESPECIFICACION_TENICA { get; set; }
    public TimeSpan? HOR_ESPECIFICACION_TENICA { get; set; }
    public bool? LOG_ESPECIFICACION_TENICA { get; set; }
    public string? STR_ESPECIFICACION_TENICA { get; set; }
    public string? VAL_ESPECIFICACION_TENICA { get; set; }
    public decimal? VAL_MINIMO { get; set; }
    public decimal? VAL_MAXIMO { get; set; }
    public int? NUM_ORDEN_PRESENTACION { get; set; }
    public bool? FLG_INCLUIR_DESCRIPCION { get; set; }
    public int? COD_GRUPO_ESPECIFICACION_TECNICA { get; set; }
    public string? DES_GRUPO_ESPECIFICACION_TECNICA { get; set; }
    public bool? FLG_REGISTRO_COMERCIAL { get; set; }
    public bool? FLG_FICHA_TECNICA { get; set; }
    public bool? FLG_CONTROL_CALIDAD { get; set; }

    // Tipo de dato de la Especificación (INGENIERIA.TIPOS_ESPECIFICACION_TECNICA) — determina
    // cuál es el ÚNICO control a mostrar para esta fila.
    public bool? FLG_LISTA { get; set; }
    public bool? FLG_ENTERO { get; set; }
    public bool? FLG_NUMERICO { get; set; }
    public bool? FLG_CADENA { get; set; }
    public bool? FLG_FECHA_HORA { get; set; }
    public bool? FLG_FECHA { get; set; }
    public bool? FLG_HORA { get; set; }
    public bool? FLG_LOGICO { get; set; }
    public bool? FLG_COLOR { get; set; }

    // Configuración adicional (INGENIERIA.ESPECIFICACIONES_TECNICAS) para Entero/Decimal/Texto.
    public int? CAN_ENTERO { get; set; }
    public int? CAN_DECIMAL { get; set; }
    public string? DES_PREFIJO { get; set; }
    public string? DES_SUFIJO { get; set; }

    // Rango (De/A) para Entero/Numérico + unidad de medida asociada.
    public bool? FLG_RANGE { get; set; }
    public string? COD_UNIDAD_MEDIDA { get; set; }
    public string? DES_UNIDAD_MEDIDA { get; set; }
    public string? COD_SIMBOLO { get; set; }
    public string COD_USUARIO_REGISTRO       { get; set; } = "";
    public string COD_ESTACION_REGISTRO      { get; set; } = "";
    public DateTime? FEC_REGISTRO            { get; set; }
    public string COD_USUARIO_ACTUALIZACION  { get; set; } = "";
    public string COD_ESTACION_ACTUALIZACION { get; set; } = "";
    public DateTime? FEC_ACTUALIZACION       { get; set; }
}

public class ProductosEspecificacionesTecnicasInsertDto
{
    public string COD_PRODUCTO { get; set; } = "";
    public string COD_ESPECIFICACION_TECNICA { get; set; } = "";
    public int? COD_ITEM_TECNICO { get; set; }
    public DateTime? FEC_ESPECIFICACION_TENICA { get; set; }
    public TimeSpan? HOR_ESPECIFICACION_TENICA { get; set; }
    public bool? LOG_ESPECIFICACION_TENICA { get; set; }
    public string? STR_ESPECIFICACION_TENICA { get; set; }
    public string? VAL_ESPECIFICACION_TENICA { get; set; }
    public decimal? VAL_MINIMO { get; set; }
    public decimal? VAL_MAXIMO { get; set; }
    public int? NUM_ORDEN_PRESENTACION { get; set; }
    public bool? FLG_INCLUIR_DESCRIPCION { get; set; }
    public int? COD_GRUPO_ESPECIFICACION_TECNICA { get; set; }
    public bool? FLG_REGISTRO_COMERCIAL { get; set; }
    public bool? FLG_FICHA_TECNICA { get; set; }
    public bool? FLG_CONTROL_CALIDAD { get; set; }
}
