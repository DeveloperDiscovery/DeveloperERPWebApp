using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Comercial;

public class ComercialTiposTransporte
{
    [Required] public string COD_TIPO_TRANSPORTE { get; set; } = string.Empty;
    [Required] public string DES_TIPO_TRANSPORTE { get; set; } = string.Empty;
    public byte[]? IMG_ICONO { get; set; }
}
