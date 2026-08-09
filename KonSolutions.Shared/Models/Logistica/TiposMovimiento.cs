using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Logistica;

/// <summary>LOGISTICA.TIPOS_MOVIMIENTO — Hijo de ClasesMovimiento (clave compuesta
/// COD_CLASE_MOVIMIENTO + COD_TIPO_MOVIMIENTO, ambos ingresados por el usuario).
/// PROC_LOGISTICA_TIPOS_MOVIMIENTO_INSERT es upsert — siempre POST.</summary>
public class TiposMovimiento
{
    public string COD_CLASE_MOVIMIENTO { get; set; } = "";
    public string COD_TIPO_MOVIMIENTO { get; set; } = "";
    public string DES_TIPO_MOVIMIENTO { get; set; } = "";
    public string? COD_TIPO_ENTIDAD_ASOCIADA { get; set; }
    public bool? FLG_POSITIVO { get; set; }
    public bool? FLG_TRANSFERENCIA { get; set; }
    public bool? FLG_COMERCIAL { get; set; }
    public bool? FLG_DEVOLUCION { get; set; }
    public bool? FLG_ANULACION { get; set; }
    public bool? FLG_CONSIGNACION { get; set; }
    public bool? FLG_AJUSTE_INVENTARIO { get; set; }
    public bool? FLG_COMPROBANTE_RELACIONADO { get; set; }
    public bool? FLG_VALOR_INVENTARIO { get; set; }
    public bool? FLG_CONSIGNA_PROYECTO { get; set; }
    public bool? FLG_CONSIGNA_CENTRO_COSTO { get; set; }
    public bool? FLG_CONSIGNA_LOTE_CONTROL { get; set; }
    public bool? FLG_DESTINO_AUTOMATICO { get; set; }
    public bool? FLG_REQUIERE_CONFIRMACION_RECEPCION { get; set; }
    public bool? FLG_CONTROL_CALIDAD { get; set; }
    public bool? FLG_REQUERIMIENTO { get; set; }
    public bool? FLG_PRODUCCION { get; set; }
    public string? COD_TIPO_MOVIMIENTO_SINCRONIZADO { get; set; }
    public string? DES_OBSERVACION { get; set; }
    // Tipo de estado que el movimiento aplica al documento relacionado (ESTANDAR.TIPOS_ESTADOS).
    public int? COD_TIPO_ESTADO_RELACIONADO { get; set; }
    public int? COD_TIPO_MOTIVO_DESTINO { get; set; }
    public int? COD_MOTIVO_DEFECTO { get; set; }
    public int? COD_TIPO_ESTADO { get; set; }
    public int? COD_ESTADO { get; set; }
    // Solo lectura (JOINs).
    public string? DES_CLASE_MOVIMIENTO { get; set; }
    public string? DES_BACKCOLOR { get; set; }
    public string? DES_ESTADO { get; set; }
    public string? DES_FORECOLOR { get; set; }
    public string? DES_MOTIVO { get; set; }
    public string? DES_TIPO_ENTIDAD { get; set; }
    public string? DES_TIPO_MOTIVO { get; set; }
    // Descripción de COD_TIPO_ESTADO_RELACIONADO (JOIN a ESTANDAR.TIPOS_ESTADOS).
    public string? DES_TIPO_ESTADO { get; set; }
    public string COD_USUARIO_REGISTRO       { get; set; } = "";
    public string COD_ESTACION_REGISTRO      { get; set; } = "";
    public DateTime? FEC_REGISTRO            { get; set; }
    public string COD_USUARIO_ACTUALIZACION  { get; set; } = "";
    public string COD_ESTACION_ACTUALIZACION { get; set; } = "";
    public DateTime? FEC_ACTUALIZACION       { get; set; }
}

public class TiposMovimientoInsertDto
{
    [Required] public string COD_CLASE_MOVIMIENTO { get; set; } = "";
    [Required] public string COD_TIPO_MOVIMIENTO { get; set; } = "";
    [Required] public string DES_TIPO_MOVIMIENTO { get; set; } = "";
    public string? COD_TIPO_ENTIDAD_ASOCIADA { get; set; }
    public bool? FLG_POSITIVO { get; set; }
    public bool? FLG_TRANSFERENCIA { get; set; }
    public bool? FLG_COMERCIAL { get; set; }
    public bool? FLG_DEVOLUCION { get; set; }
    public bool? FLG_ANULACION { get; set; }
    public bool? FLG_CONSIGNACION { get; set; }
    public bool? FLG_AJUSTE_INVENTARIO { get; set; }
    public bool? FLG_COMPROBANTE_RELACIONADO { get; set; }
    public bool? FLG_VALOR_INVENTARIO { get; set; }
    public bool? FLG_CONSIGNA_PROYECTO { get; set; }
    public bool? FLG_CONSIGNA_CENTRO_COSTO { get; set; }
    public bool? FLG_CONSIGNA_LOTE_CONTROL { get; set; }
    public bool? FLG_DESTINO_AUTOMATICO { get; set; }
    public bool? FLG_REQUIERE_CONFIRMACION_RECEPCION { get; set; }
    public bool? FLG_CONTROL_CALIDAD { get; set; }
    public bool? FLG_REQUERIMIENTO { get; set; }
    public bool? FLG_PRODUCCION { get; set; }
    public string? COD_TIPO_MOVIMIENTO_SINCRONIZADO { get; set; }
    public string? DES_OBSERVACION { get; set; }
    public int? COD_TIPO_ESTADO_RELACIONADO { get; set; }
    public int? COD_TIPO_MOTIVO_DESTINO { get; set; }
    public int? COD_MOTIVO_DEFECTO { get; set; }
    public int? COD_TIPO_ESTADO { get; set; }
    public int? COD_ESTADO { get; set; }
}
