using System.ComponentModel.DataAnnotations;
namespace KONSolutions.Shared.Models.Logistica;

/// <summary>LOGISTICA.LOTES_CONTROL_DETALLE — los productos que componen el lote.</summary>
public class LotesControlDetalle
{
    public string   NUM_LOTE_CONTROL { get; set; } = string.Empty;
    public string   COD_PRODUCTO     { get; set; } = string.Empty;
    public decimal? VAL_TOTAL        { get; set; }
    public decimal? CAN_PRODUCTO     { get; set; }

    // Vienen resueltas por los JOIN del procedimiento: sólo se muestran.
    public string? DES_LOTE_CONTROL { get; set; }
    public string? DES_PRODUCTO     { get; set; }
    public string? DES_DETALLADA    { get; set; }
}
