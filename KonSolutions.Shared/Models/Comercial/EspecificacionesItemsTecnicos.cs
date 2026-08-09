namespace KONSolutions.Shared.Models.Comercial;

/// <summary>
/// Fila genérica del árbol de INGENIERIA.ESPECIFICACIONES_ITEMS_TECNICOS de una entidad,
/// devuelta por PROC_INGENIERIA_ESPECIFICACIONES_TECNICAS_TREELIST — mezcla 3 niveles
/// (Entidad, Especificación Técnica, Ítem Técnico) bajo una forma clave/padre genérica.
/// </summary>
public class EspecificacionesItemsTecnicosTreeList
{
    public string COD_KEY { get; set; } = string.Empty;
    public string? DES_KEY { get; set; }
    public string? COD_KEY_FATHER { get; set; }
    public string COD_KEY_TYPE { get; set; } = string.Empty;
    public string? DES_KEY_TYPE { get; set; }
    public string? COD_UNIDAD_MEDIDA { get; set; }
    public string? DES_UNIDAD_MEDIDA { get; set; }
    public decimal? VAL_NUMERICO { get; set; }
    public string? COD_EP { get; set; }
    public string? DES_EP { get; set; }
    public string? COD_ITEM { get; set; }
    public string? DES_ITEM { get; set; }
    public string? COD_EP_FATHER { get; set; }
    public string? DES_EP_FATHER { get; set; }
    public string? COD_ITEM_FATHER { get; set; }
    public string? DES_ITEM_FATHER { get; set; }
    public bool? FLG_HABILITADO { get; set; }
    public bool? FLG_DEFAULT { get; set; }
    public string? DES_COLOR { get; set; }
    public int? NUM_ORDEN_PRESENTACION { get; set; }
    public bool? FLG_COLOR { get; set; }
    public bool? FLG_VALOR { get; set; }
    public bool? FLG_IS_CHILDREN { get; set; }

    /// <summary>Nodos hijos — se arma en memoria a partir de COD_KEY_FATHER, no viene del API.</summary>
    public List<EspecificacionesItemsTecnicosTreeList> Hijos { get; set; } = new();
}

/// <summary>
/// Fila devuelta por PROC_INGENIERIA_ESPECFICACIONES_ITEMS_TECNICOS_COMBOXDEPENDEINTES —
/// Ítems Técnicos de una Especificación, filtrados por la Entidad dueña del Producto y,
/// si la Especificación depende de otra (ej. MODELO depende de MARCA), por el Ítem elegido
/// en la Especificación padre.
/// </summary>
public class EspecificacionesItemsTecnicosComboDependiente
{
    public int COD_ITEM_TECNICO { get; set; }
    public string? DES_ITEM_TECNICO { get; set; }
    public decimal? VAL_NUMERICO { get; set; }
    public bool? FLG_DEFAULT { get; set; }
    public decimal? COD_ENTIDAD_DEPENDIENTE { get; set; }
    public string COD_ESPECIFICACION_TECNICA { get; set; } = string.Empty;
    public string? COD_ESPECIFICACION_TECNICA_DEPENDIENTE { get; set; }
    public int? COD_ITEM_TECNICO_DEPENDIENTE { get; set; }
    public int? NUM_ORDEN_PRESENTACION { get; set; }
    public string? DES_COLOR { get; set; }
    public string? COD_UNIDAD_MEDIDA { get; set; }
    public string? DES_UNIDAD_MEDIDA { get; set; }
    public string? COD_SIMBOLO { get; set; }
}

/// <summary>DTO para el upsert (Insert/Update) de un Ítem Técnico — PROC_INGENIERIA_ESPECIFICACIONES_TECNICAS_ITEMS_INSERT.</summary>
public class EspecificacionesItemsTecnicosUpdateDto
{
    public string COD_ESPECIFICACION_TECNICA { get; set; } = string.Empty;
    public string COD_ITEM_TECNICO { get; set; } = string.Empty;
    public string? DES_ITEM_TECNICO { get; set; }
    public decimal? COD_ENTIDAD_DEPENDIENTE { get; set; }
    public string? COD_ESPECIFICACION_TECNICA_DEPENDIENTE { get; set; }
    public string? COD_ITEM_TECNICO_DEPENDIENTE { get; set; }
    public bool? FLG_HABILITADO { get; set; }
    public bool? FLG_DEFAULT { get; set; }
    public decimal? VAL_NUMERICO { get; set; }
    public string? DES_COLOR { get; set; }
    public int? NUM_ORDEN_PRESENTACION { get; set; }
}
