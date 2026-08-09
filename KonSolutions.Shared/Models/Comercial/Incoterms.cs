using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Comercial;

public class Incoterms
{
    [Required] public string DES_INCOTERM { get; set; } = string.Empty;
    public string COD_INCOTERM { get; set; } = string.Empty;
    public byte[]? IMG_ICONO { get; set; }
    public int?    NUM_ORDEN_PRESENTACION { get; set; }
}
