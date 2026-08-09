using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Comercial;

public class CondicionesPago
{
    public int?    COD_CONDICION_PAGO   { get; set; }
    [Required] public string DES_CONDICION_PAGO { get; set; } = string.Empty;
    public bool?   FLG_CREDITO          { get; set; }
    public int?    CAN_DIAS             { get; set; }
    public bool?   FLG_DIAS_CALENDARIO  { get; set; }
    public byte[]? IMG_ICONO            { get; set; }
}
