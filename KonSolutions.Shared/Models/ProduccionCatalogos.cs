using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Produccion;

// ───────────── PRODUCCION.CLASES_PROCESO ─────────────
public class ClasesProceso
{
    [Required] public string COD_CLASE_PROCESO { get; set; } = "";
    [Required] public string DES_CLASE_PROCESO { get; set; } = "";
    public byte[]? IMG_ICONO { get; set; }
    public int?  NUM_ORDEN_PRESENTACION { get; set; }
    public bool? FLG_GENERA_GRUPO_REQUERIMIENTO { get; set; }
}

// ───────────── PRODUCCION.PROCESOS ─────────────
public class Procesos
{
    [Required] public string COD_PROCESO { get; set; } = "";
    [Required] public string DES_PROCESO { get; set; } = "";
    public string? DES_ALIAS { get; set; }
    public string? COD_CLASE_PROCESO { get; set; }
    public bool? FLG_ORDEN_FABRICACION { get; set; }
    public string? DES_NOMBRE_ORDEN_PRODUCCION { get; set; }
    public string? DES_NOMBRE_ORDEN_PRODUCCION_ITEM { get; set; }
    public string? DES_NOMBRE_ORDEN_PRODUCCION_UNIDAD { get; set; }
    public string? DES_EQUIPO_PRODUCCION_PRINCIPAL { get; set; }
    public string? DES_LINEA_PRODUCCION { get; set; }
    public bool? FLG_EMBALAJE_FINAL { get; set; }
    public bool? FLG_CONTROL_CALIDAD { get; set; }
    public bool? FLG_PROCESO_MANTENIMIENTO { get; set; }
    public string? COD_TIPO_MERMA { get; set; }
    public int?  VAL_ULTIMO_CORRELATIVO { get; set; }
    public byte[]? IMG_ICONO { get; set; }
    public int?  COD_TIPO_ESTADO { get; set; }
    public int?  COD_ESTADO { get; set; }

    // ── Solo lectura: traídos por el SHOWALL/SEARCH vía JOIN ──
    public string DES_CLASE_PROCESO { get; set; } = "";
    public string DES_BACKCOLOR     { get; set; } = "";
    public string DES_ESTADO        { get; set; } = "";
    public string DES_FORECOLOR     { get; set; } = "";
    public string DES_TIPO_MERMA    { get; set; } = "";
    public string COD_USUARIO_REGISTRO       { get; set; } = "";
    public string COD_ESTACION_REGISTRO      { get; set; } = "";
    public DateTime? FEC_REGISTRO            { get; set; }
    public string COD_USUARIO_ACTUALIZACION  { get; set; } = "";
    public string COD_ESTACION_ACTUALIZACION { get; set; } = "";
    public DateTime? FEC_ACTUALIZACION       { get; set; }
}

// ───────────── PRODUCCION.SUB_PROCESOS (hijo de PROCESOS) ─────────────
public class SubProcesos
{
    [Required] public string COD_PROCESO { get; set; } = "";
    [Required] public string COD_SUB_PROCESO { get; set; } = "";
    [Required] public string DES_SUB_PROCESO { get; set; } = "";
    public string? DES_ALIAS { get; set; }
    public bool? FLG_ORDEN_FABRICACION { get; set; }
    public string? DES_NOMBRE_ORDEN_PRODUCCION { get; set; }
    public string? DES_NOMBRE_ORDEN_PRODUCCION_ITEM { get; set; }
    public string? DES_NOMBRE_ORDEN_PRODUCCION_UNIDAD { get; set; }
    public string? DES_EQUIPO_PRODUCCION_PRINCIPAL { get; set; }
    public string? DES_LINEA_PRODUCCION { get; set; }
    public bool? FLG_EMBALAJE_FINAL { get; set; }
    public bool? FLG_CONTROL_CALIDAD { get; set; }
    public string? COD_TIPO_MERMA { get; set; }
    public int?  VAL_ULTIMO_CORRELATIVO { get; set; }
    public int?  VAL_ULTIMO_CORRELATIVO_CONTENEDOR { get; set; }
    public byte[]? IMG_ICONO { get; set; }
    public int?  COD_TIPO_ESTADO { get; set; }
    public int?  COD_ESTADO { get; set; }

    // ── Solo lectura ──
    public string DES_BACKCOLOR  { get; set; } = "";
    public string DES_ESTADO     { get; set; } = "";
    public string DES_FORECOLOR  { get; set; } = "";
    public string DES_PROCESO    { get; set; } = "";
    public string DES_TIPO_MERMA { get; set; } = "";
    public string COD_USUARIO_REGISTRO       { get; set; } = "";
    public string COD_ESTACION_REGISTRO      { get; set; } = "";
    public DateTime? FEC_REGISTRO            { get; set; }
    public string COD_USUARIO_ACTUALIZACION  { get; set; } = "";
    public string COD_ESTACION_ACTUALIZACION { get; set; } = "";
    public DateTime? FEC_ACTUALIZACION       { get; set; }
}

// ───────────── PRODUCCION.TIPOS_LINEA_PRODUCCION ─────────────
public class TiposLineaProduccion
{
    [Required] public string COD_TIPO_LINEA_PRODUCCION { get; set; } = "";
    [Required] public string DES_TIPO_LINEA_PRODUCCION { get; set; } = "";
    public bool? FLG_PRODUCCION { get; set; }
    public bool? FLG_PLANTA_PROPIA { get; set; }
    public byte[]? IMG_PICTURE { get; set; }
}
