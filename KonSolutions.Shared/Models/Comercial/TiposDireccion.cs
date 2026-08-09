using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Comercial;

public class TiposDireccion
{
    public int?    COD_TIPO_DIRECCION { get; set; }
    [Required] public string DES_TIPO_DIRECCION { get; set; } = string.Empty;
    public bool?   FLG_COBRANZA { get; set; }
    public bool?   FLG_ENTREGA  { get; set; }
}
