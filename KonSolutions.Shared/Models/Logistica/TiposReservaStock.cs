using System.ComponentModel.DataAnnotations;
namespace KONSolutions.Shared.Models.Logistica;

/// <summary>LOGISTICA.TIPOS_RESERVA_STOCK</summary>
public class TiposReservaStock
{
    public string COD_TIPO_RESERVA { get; set; } = string.Empty;
    public string DES_TIPO_RESERVA { get; set; } = string.Empty;
    public bool? FLG_VENTA { get; set; }
    public bool? FLG_PRODUCCION { get; set; }
}
