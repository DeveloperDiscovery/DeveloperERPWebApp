using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Comercial;

public class FormasPago
{
    public int?    COD_FORMA_PAGO       { get; set; }
    [Required] public string DES_FORMA_PAGO { get; set; } = string.Empty;
    public bool?   FLG_EFECTIVO         { get; set; }
    [Required] public string COD_FORMA_PAGO_SUNAT { get; set; } = string.Empty;
    public byte[]? IMG_ICONO            { get; set; }
}
