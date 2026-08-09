using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Sunat;

public class RegimenesTributarios
{
    public int? COD_REGIMEN_TRIBUTARIO { get; set; }
    [Required] public string DES_REGIMEN_TRIBUTARIO { get; set; } = string.Empty;
    [Required] public string COD_TIPO_COMPROBANTE_PAGO { get; set; } = string.Empty;
    public bool? FLG_RETENCION_TRIBUTO { get; set; }
    public bool? FLG_DEPOSITO_CUENTA { get; set; }
    public bool? FLG_PERIODO { get; set; }
    public bool? FLG_INCREMENTA { get; set; }
    public bool? FLG_DEPENDE_PRODUCTO { get; set; }
    public bool? FLG_RESTRICCION { get; set; }
    public bool? FLG_PROVISION { get; set; }
    public int? NUM_PRIORIDAD { get; set; }
    public int? NUM_DECIMALES_REDONDEO { get; set; }
    public string COD_CLASE_OPERACION { get; set; } = string.Empty;
    public string COD_TIPO_OPERACION { get; set; } = string.Empty;
    public int? NUM_DIAS { get; set; }
    public decimal? VAL_MINIMO { get; set; }
    public decimal? TAS_REGIMEN_TRIBUTARIO { get; set; }
    [Required] public string COD_TIPO_IMPUESTO { get; set; } = string.Empty;
    [Required] public string COD_TIPO_VALOR_CALCULO { get; set; } = string.Empty;
    [Required] public string COD_MONEDA { get; set; } = string.Empty;
    public int? COD_TIPO_ESTADO { get; set; }
    public int? COD_ESTADO { get; set; }
    public string? COD_USUARIO_REGISTRO { get; set; }
    public string? COD_ESTACION_REGISTRO { get; set; }
    public DateTime? FEC_REGISTRO { get; set; }
    public string? COD_USUARIO_ACTUALIZACION { get; set; }
    public string? COD_ESTACION_ACTUALIZACION { get; set; }
    public DateTime? FEC_ACTUALIZACION { get; set; }
    public string? DES_BACKCOLOR { get; set; }
    public string? DES_ESTADO { get; set; }
    public string? DES_FORECOLOR { get; set; }
    public string? DES_MONEDA { get; set; }
    public string? DES_TIPO_COMPROBANTE_PAGO { get; set; }
    public string? DES_TIPO_IMPUESTO { get; set; }
    public string? DES_TIPO_OPERACION { get; set; }
    public string? DES_TIPO_VALOR_CALCULO { get; set; }
}
