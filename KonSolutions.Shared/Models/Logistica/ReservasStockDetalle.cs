using System.ComponentModel.DataAnnotations;
namespace KONSolutions.Shared.Models.Logistica;

/// <summary>LOGISTICA.RESERVAS_STOCK_DETALLE</summary>
public class ReservasStockDetalle
{
    public int? NUM_RESERVA_STOCK { get; set; }
    public int? NUM_SECUENCIA { get; set; }
    public string COD_PRODUCTO { get; set; } = string.Empty;
    public string? NUM_LOTE_CONTROL { get; set; }
    public string? COD_UNIDAD_MEDIDA { get; set; }
    public decimal? CAN_PRODUCTO { get; set; }
    public string? COD_UNIDAD_MEDIDA_SECUNDARIA { get; set; }
    public decimal? CAN_PRODUCTO_SECUNDARIA { get; set; }
    public decimal? CAN_PRODUCTO_ATENDIDA { get; set; }
    public decimal? CAN_PRODUCTO_ATENDIDA_SECUNDARIA { get; set; }

    // Vienen resueltas por los JOIN del procedimiento: sólo se muestran.
    public string? DES_PRODUCTO { get; set; }
    public string? DES_DETALLADA { get; set; }
    public string? DES_UNIDAD_MEDIDA { get; set; }
    public string? DES_LOTE_CONTROL { get; set; }

    // Vienen resueltas por los JOIN del procedimiento: solo se muestran.
    public string DES_RESERVA_STOCK { get; set; } = string.Empty;
    public string DES_OBSERVACION { get; set; } = string.Empty;
}
