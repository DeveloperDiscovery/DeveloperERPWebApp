namespace KONSolutions.Shared.Models.Logistica;

/// <summary>Los dos resultados de PROC_MOVIMIENTOS_ALMACEN_REQUERIMIENTO_STOCK_DETALLE_DISPONIBLES
/// en un solo objeto. Espejo de MovimientoAlmacenRequerimientoCobertura de la API.</summary>
public class CoberturaRequerimientoResultado
{
    public List<CoberturaRequerimientoLinea> Lineas { get; set; } = new();
    public List<CoberturaStockSeleccionado> Stock { get; set; } = new();
}

/// <summary>Resultado 1: lo disponible por línea del requerimiento.</summary>
public class CoberturaRequerimientoLinea
{
    public int NUM_REQUERIMIENTO_STOCK { get; set; }
    public int NUM_SECUENCIA { get; set; }
    public string COD_PRODUCTO { get; set; } = string.Empty;
    public string? DES_PRODUCTO { get; set; }
    public decimal CAN_PRODUCTO { get; set; }
    public string? COD_UNIDAD_MEDIDA { get; set; }
    public decimal? CAN_PRODUCTO_SECUNDARIA { get; set; }
    public string? COD_UNIDAD_MEDIDA_SECUNDARIA { get; set; }
    public decimal CAN_ATENDER { get; set; }
    public decimal CAN_PENDIENTE_ATENDER { get; set; }
    /// <summary>POR REVISAR | PARCIAL | COMPLETO | ASIGNADA</summary>
    public string DES_COBERTURA { get; set; } = "POR REVISAR";
}

/// <summary>Resultado 2: stock a despachar y reservas de origen, por secuencia.</summary>
public class CoberturaStockSeleccionado
{
    public int NUM_SECUENCIA { get; set; }
    public string? DES_LOTE_CONTROL { get; set; }
    public string? DES_PROCESO { get; set; }
    public int? NUM_ORDEN_PRODUCCION { get; set; }
    public int? NUM_ORDEN_PRODUCCION_ITEM { get; set; }
    public int? NUM_ORDEN_PRODUCCION_UNIDAD { get; set; }
    public bool FLG_RESERVA { get; set; }
    public decimal CAN_PRODUCTO { get; set; }
    public decimal? CAN_PRODUCTO_SECUNDARIA { get; set; }
    public int? NUM_RESERVA_STOCK { get; set; }
    public int? NUM_SECUENCIA_RESERVA { get; set; }
    public int NUM_ORDEN_SECUENCIA { get; set; }
}
