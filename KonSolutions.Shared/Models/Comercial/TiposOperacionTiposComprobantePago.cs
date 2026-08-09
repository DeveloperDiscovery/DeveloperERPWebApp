using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Comercial;

public class TiposOperacionTiposComprobantePago
{
    public string COD_CLASE_OPERACION { get; set; } = string.Empty;
    public string COD_TIPO_OPERACION { get; set; } = string.Empty;
    [Required] public string COD_TIPO_COMPROBANTE_PAGO { get; set; } = string.Empty;
    public string? COD_USUARIO_REGISTRO { get; set; }
    public string? COD_ESTACION_REGISTRO { get; set; }
    public DateTime? FEC_REGISTRO { get; set; }
    public string? DES_TIPO_COMPROBANTE_PAGO { get; set; }
    public string? DES_TIPO_OPERACION { get; set; }
}
