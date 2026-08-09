using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Comercial;

public class FasesIncoterm
{
    [Required] public string COD_FASE_INCOTERM { get; set; } = string.Empty;
    [Required] public string DES_FASE_INCOTERM { get; set; } = string.Empty;
    public byte[]? IMG_ICONO { get; set; }
}
