using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Comercial;

/// <summary>Catálogo COMERCIAL.TIPOS_PEDIDO_COMERCIAL (COD identity).</summary>
public class TiposPedidoComercial
{
    public int?    COD_TIPO_PEDIDO_COMERCIAL  { get; set; }
    [Required] public string DES_TIPO_PEDIDO_COMERCIAL { get; set; } = string.Empty;
    [Required] public string COD_TIPO_ENTIDAD           { get; set; } = string.Empty;
    [Required] public string COD_TIPO_ENTIDAD_VENDEDORA { get; set; } = string.Empty;
    public bool?   FLG_PEDIDO_FORMAL          { get; set; }
    public bool?   FLG_BIENES                 { get; set; }
    public string? COD_TIPO_COMPROBANTE_PAGO_INTERNO { get; set; }

    // Solo lectura (vienen del JOIN en SHOWALL/SEARCH/SHOWBYID).
    public string? DES_TIPO_ENTIDAD           { get; set; }
    public string? DES_TIPO_ENTIDAD_VENDEDORA { get; set; }
    public string? DES_TIPO_COMPROBANTE_PAGO  { get; set; }
}
