using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Comercial;

public class TiposDescuento
{
    public int?     COD_TIPO_DESCUENTO { get; set; }
    [Required] public string DES_TIPO_DESCUENTO { get; set; } = string.Empty;
    public decimal? TAS_DESCUENTO      { get; set; }
}
