using System.ComponentModel.DataAnnotations;
using KONSolutions.Shared.Common;

namespace KONSolutions.Shared.Models.Ingenieria;

// ───────────── INGENIERIA.TIPOS_ESPECIFICACION_TECNICA ─────────────
public class TiposEspecificacionTecnica
{
    [Required] public string COD_TIPO_ESPECIFICACION_TECNICA { get; set; } = "";
    [Required] public string DES_TIPO_ESPECIFICACION_TECNICA { get; set; } = "";
    public bool? FLG_LISTA               { get; set; }
    public bool? FLG_ENTERO              { get; set; }
    public bool? FLG_NUMERICO            { get; set; }
    public bool? FLG_CADENA              { get; set; }
    public bool? FLG_FECHA_HORA          { get; set; }
    public bool? FLG_FECHA               { get; set; }
    public bool? FLG_HORA                { get; set; }
    public bool? FLG_LOGICO              { get; set; }
    public bool? FLG_COLOR               { get; set; }
    public int?  NUM_ORDEN_PRESENTACION  { get; set; }
}

// ───────────── INGENIERIA.ESPECIFICACIONES_TECNICAS ─────────────
public class EspecificacionesTecnicas
{
    public string COD_ESPECIFICACION_TECNICA { get; set; } = "";
    [Required] public string DES_ESPECIFICACION_TECNICA { get; set; } = "";
    [Required] public string DES_ESPECIFICACION_TECNICA_SINGULAR { get; set; } = "";
    public string? COD_ESPECIFICACION_TECNICA_PADRE { get; set; }
    [Required] public string COD_TIPO_ESPECIFICACION_TECNICA { get; set; } = "";
    public bool? FLG_HAS_CHILDREN          { get; set; }
    public bool? FLG_LIST                  { get; set; }
    public bool? FLG_IS_CHILDREN           { get; set; }
    public bool? FLG_DEPENDS_ENTITY        { get; set; }
    public bool? FLG_MULTIPLE_CHOICE       { get; set; }
    public bool? FLG_ITEMS_WITH_VALUE      { get; set; }
    public bool? FLG_PRODUCT_ASSEMBLY      { get; set; }
    public bool? FLG_QUALITY_CONTROL       { get; set; }
    public bool? FLG_OBSERVACION           { get; set; }
    public bool? FLG_RANGE                 { get; set; }
    public bool? FLG_EDITABLE_ORDER_TAKING { get; set; }
    public bool? FLG_IS_COLOR              { get; set; }
    public string COD_CLASE_DEFECTO_TECNICO        { get; set; } = "";
    public string COD_TIPO_DEFECTO_TECNICO_DEFECTO { get; set; } = "";
    public string DES_PREFIJO              { get; set; } = "";
    public string DES_SUFIJO               { get; set; } = "";
    public int?  CAN_ENTERO                { get; set; }
    public int?  CAN_DECIMAL               { get; set; }
    public string COD_UNIDAD_MEDIDA        { get; set; } = "";
    public string COD_UNIDAD_MEDIDA_DIVISORA { get; set; } = "";
    public string COD_UNIDAD_MEDIDA_ITEM { get; set; } = "";
    public int?  NUM_ORDEN_PRESENTACION    { get; set; }
    public byte[]? IMG_ICONO               { get; set; }
    public int?  COD_TIPO_ESTADO           { get; set; }
    public int?  COD_ESTADO                { get; set; }
    public string DES_BACKCOLOR            { get; set; } = "";
    public string DES_ESTADO               { get; set; } = "";
    public string DES_FORECOLOR            { get; set; } = "";
    public string DES_CLASE_DEFECTO_TECNICO { get; set; } = "";
    public string DES_DEFECTO_TECNICO      { get; set; } = "";
    public string DES_UNIDAD_MEDIDA        { get; set; } = "";
    public string DES_TIPO_ESPECIFICACION_TECNICA { get; set; } = "";
    public string COD_USUARIO_REGISTRO       { get; set; } = "";
    public string COD_ESTACION_REGISTRO      { get; set; } = "";
    public DateTime? FEC_REGISTRO            { get; set; }
    public string COD_USUARIO_ACTUALIZACION  { get; set; } = "";
    public string COD_ESTACION_ACTUALIZACION { get; set; } = "";
    public DateTime? FEC_ACTUALIZACION       { get; set; }

    /// <summary>Nodos hijos — se arma en memoria a partir de COD_ESPECIFICACION_TECNICA_PADRE, no viene del API.</summary>
    public List<EspecificacionesTecnicas> Hijos { get; set; } = new();
}

public class EspecificacionesTecnicasLoadCombosResult
{
    public int                CodTipoEstado       { get; set; }
    public List<ComboboxItem> ClasesDefecto       { get; set; } = new();
    public List<ComboboxItem> TiposEspecificacion { get; set; } = new();
    public List<EstadoItem>   Estados             { get; set; } = new();
    public List<ComboboxItem> UnidadesMedida      { get; set; } = new();
}

// ───────────── INGENIERIA.GRUPOS_FAMILIAS_SUB_FAMILIAS_ESPECIFICACION_TECNICA ─────────────
// Configuración de qué Especificaciones Técnicas aplican a una Sub Familia de Producto.
// Solo se asigna a nivel de Especificación Técnica completa (COD_ITEM_TECNICO siempre
// queda NULL — no se asigna a nivel de Ítem puntual).
public class GruposFamiliasSubFamiliasEspecificacionTecnica
{
    public string COD_GRUPO_PRODUCTO { get; set; } = "";
    public string COD_FAMILIA_PRODUCTO { get; set; } = "";
    public string COD_SUB_FAMILIA_PRODUCTO { get; set; } = "";
    public string COD_ESPECIFICACION_TECNICA { get; set; } = "";
    public int? COD_ITEM_TECNICO { get; set; }
    public string? VAL_ESPECIFICACION_TENICA { get; set; }
    public decimal? VAL_MINIMO { get; set; }
    public decimal? VAL_MAXIMO { get; set; }
    public int? NUM_ORDEN_PRESENTACION { get; set; }
    public bool? FLG_INCLUIR_DESCRIPCION { get; set; }
    public bool? FLG_REGISTRO_COMERCIAL { get; set; }
    public bool? FLG_FICHA_TECNICA { get; set; }
    public bool? FLG_CONTROL_CALIDAD { get; set; }
    public string? COD_TIPO_REGISTRO_DATOS { get; set; }
    public int? COD_GRUPO_ESPECIFICACION_TECNICA { get; set; }
    public int? COD_TIPO_ESTADO { get; set; }
    public int? COD_ESTADO { get; set; }
    public string DES_ESTADO { get; set; } = "";
    public string DES_BACKCOLOR { get; set; } = "";
    public string DES_FORECOLOR { get; set; } = "";
    public string DES_ESPECIFICACION_TECNICA { get; set; } = "";
    public string? DES_SUB_FAMILIA_PRODUCTO { get; set; }
    public string? DES_TIPO_REGISTRO_DATOS { get; set; }
    public string? DES_GRUPO_ESPECIFICACION_TECNICA { get; set; }
    public string COD_USUARIO_REGISTRO       { get; set; } = "";
    public string COD_ESTACION_REGISTRO      { get; set; } = "";
    public DateTime? FEC_REGISTRO            { get; set; }
    public string COD_USUARIO_ACTUALIZACION  { get; set; } = "";
    public string COD_ESTACION_ACTUALIZACION { get; set; } = "";
    public DateTime? FEC_ACTUALIZACION       { get; set; }
}

public class GruposFamiliasSubFamiliasEspecificacionTecnicaInsertDto
{
    [Required] public string COD_GRUPO_PRODUCTO { get; set; } = "";
    [Required] public string COD_FAMILIA_PRODUCTO { get; set; } = "";
    [Required] public string COD_SUB_FAMILIA_PRODUCTO { get; set; } = "";
    [Required] public string COD_ESPECIFICACION_TECNICA { get; set; } = "";
    public string? VAL_ESPECIFICACION_TENICA { get; set; }
    public decimal? VAL_MINIMO { get; set; }
    public decimal? VAL_MAXIMO { get; set; }
    public int? NUM_ORDEN_PRESENTACION { get; set; }
    public bool? FLG_INCLUIR_DESCRIPCION { get; set; }
    public bool? FLG_REGISTRO_COMERCIAL { get; set; }
    public bool? FLG_FICHA_TECNICA { get; set; }
    public bool? FLG_CONTROL_CALIDAD { get; set; }
    public string? COD_TIPO_REGISTRO_DATOS { get; set; }
    public int? COD_GRUPO_ESPECIFICACION_TECNICA { get; set; }
    public int? COD_TIPO_ESTADO { get; set; }
    public int? COD_ESTADO { get; set; }
}

/// <summary>
/// Fila genérica del árbol de configuración — 2 niveles (Especificación Técnica / Ítem
/// Técnico) bajo el mismo patrón COD_KEY/COD_KEY_FATHER usado en el árbol de Especificaciones/
/// Ítems Técnicos por Entidad. Solo los nodos de nivel 1 (COD_KEY_TYPE == "1") son
/// asignables — los Ítems son solo informativos.
/// </summary>
public class GruposFamiliasSubFamiliasEspecificacionTecnicaTreeList
{
    public string COD_KEY { get; set; } = "";
    public string? DES_KEY { get; set; }
    public string? COD_KEY_FATHER { get; set; }
    public string COD_KEY_TYPE { get; set; } = "";
    public string? DES_KEY_TYPE { get; set; }
    public bool? FLG_ASIGNADO { get; set; }
    public string? VAL_ESPECIFICACION_TENICA { get; set; }
    public decimal? VAL_MINIMO { get; set; }
    public decimal? VAL_MAXIMO { get; set; }
    public int? COD_TIPO_ESTADO { get; set; }
    public int? COD_ESTADO { get; set; }
    public string? DES_ESTADO { get; set; }
    public string? DES_BACKCOLOR { get; set; }
    public string? DES_FORECOLOR { get; set; }
    public int? NUM_ORDEN_PRESENTACION { get; set; }
    public bool? FLG_INCLUIR_DESCRIPCION { get; set; }
    public bool? FLG_REGISTRO_COMERCIAL { get; set; }
    public bool? FLG_FICHA_TECNICA { get; set; }
    public bool? FLG_CONTROL_CALIDAD { get; set; }
    public string? COD_TIPO_ESPECIFICACION_TECNICA { get; set; }
    public string? DES_TIPO_ESPECIFICACION_TECNICA { get; set; }
    public string? DES_ESPECIFICACION_TECNICA_SINGULAR { get; set; }
    public string? COD_TIPO_REGISTRO_DATOS { get; set; }
    public string? DES_TIPO_REGISTRO_DATOS { get; set; }
    public int? COD_GRUPO_ESPECIFICACION_TECNICA { get; set; }
    public string? DES_GRUPO_ESPECIFICACION_TECNICA { get; set; }

    /// <summary>Nodos hijos — se arma en memoria a partir de COD_KEY_FATHER, no viene del API.</summary>
    public List<GruposFamiliasSubFamiliasEspecificacionTecnicaTreeList> Hijos { get; set; } = new();
}

/// <summary>Una Sub Familia destino al copiar las Especificaciones Técnicas asignadas.</summary>
public class GruposFamiliasSubFamiliasEspecificacionTecnicaDestino
{
    [Required] public string COD_GRUPO_PRODUCTO { get; set; } = "";
    [Required] public string COD_FAMILIA_PRODUCTO { get; set; } = "";
    [Required] public string COD_SUB_FAMILIA_PRODUCTO { get; set; } = "";
}

public class GruposFamiliasSubFamiliasEspecificacionTecnicaCopiarDto
{
    [Required] public string COD_GRUPO_PRODUCTO { get; set; } = "";
    [Required] public string COD_FAMILIA_PRODUCTO { get; set; } = "";
    [Required] public string COD_SUB_FAMILIA_PRODUCTO { get; set; } = "";
    public List<GruposFamiliasSubFamiliasEspecificacionTecnicaDestino> Destinos { get; set; } = new();
}

public class GruposFamiliasSubFamiliasEspecificacionTecnicaCopiarResultado
{
    public int TotalCopiados { get; set; }
    public int TotalErrores { get; set; }
    public List<string> Detalles { get; set; } = new();
}
