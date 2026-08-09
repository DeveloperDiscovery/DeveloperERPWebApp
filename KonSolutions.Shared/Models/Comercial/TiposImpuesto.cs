using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Comercial;

public class TiposImpuesto
{
    [Required] public string COD_TIPO_IMPUESTO { get; set; } = string.Empty;
    [Required] public string DES_TIPO_IMPUESTO { get; set; } = string.Empty;
    [Required] public int? COD_TIPO_IMPUESTO_SUNAT { get; set; }
}
