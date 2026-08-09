using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Comercial;

public class TiposPos
{
    [Required] public string COD_TIPO_POS { get; set; } = string.Empty;
    [Required] public string DES_TIPO_POS { get; set; } = string.Empty;
    public byte[]? IMG_PICTURE                     { get; set; }
    public bool?   FLG_VENTA_MENUDEO               { get; set; }
    public bool?   FLG_PEDIDO_VENTA_RESTAURANTE     { get; set; }
    public bool?   FLG_PEDIDO_VENTA_POR_MAYOR       { get; set; }
    public string COD_USUARIO_REGISTRO  { get; set; } = "";
    public string COD_ESTACION_REGISTRO { get; set; } = "";
    public DateTime? FEC_REGISTRO       { get; set; }
}
