using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Comercial;

public class TiposOperacionCalculo
{
    public string COD_CLASE_OPERACION { get; set; } = string.Empty;
    public string COD_TIPO_OPERACION { get; set; } = string.Empty;
    [Required] public string COD_TIPO_VALOR_CALCULO { get; set; } = string.Empty;
    public int? NUM_ORDEN_PRESENTACION { get; set; }
    public bool? FLG_CALCULO_INVERSO { get; set; }
    public bool? FLG_MOSTRAR_COMPROBANTE { get; set; }
    public bool? FLG_VALOR_MOSTRAR { get; set; }
    public bool? FLG_POSITIVO { get; set; }
    public string? COD_CUENTA_NATURALEZA { get; set; }
    public string? COD_CUENTA_CONTABLE { get; set; }
    public string? COD_USUARIO_REGISTRO { get; set; }
    public string? COD_ESTACION_REGISTRO { get; set; }
    public DateTime? FEC_REGISTRO { get; set; }
    public string? DES_CUENTA_NATURALEZA { get; set; }
    public string? DES_CUENTA_NATURALEZA_ALTERNATIVA { get; set; }
    public string? DES_TIPO_OPERACION { get; set; }
    public string? DES_TIPO_VALOR_CALCULO { get; set; }
    // Provienen del JOIN con COMERCIAL.TIPOS_VALOR_CALCULO — solo lectura.
    public bool? FLG_CALCULA { get; set; }
    public bool? FLG_IMPUESTO { get; set; }
    public bool? FLG_REGIMEN_TRIBUTARIO { get; set; }
}
