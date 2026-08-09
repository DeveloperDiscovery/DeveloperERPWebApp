using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Comercial;

public class TiposImpuestoValores
{
    [Required] public string COD_TIPO_IMPUESTO { get; set; } = string.Empty;
    public DateTime? FEC_INICIO   { get; set; }
    public DateTime? FEC_FINAL    { get; set; }
    public decimal?  TAS_IMPUESTO { get; set; }
    public decimal?  VAL_IMPUESTO { get; set; }

    // Solo lectura (viene del JOIN).
    public string? DES_TIPO_IMPUESTO { get; set; }
    public string COD_USUARIO_REGISTRO       { get; set; } = "";
    public string COD_ESTACION_REGISTRO      { get; set; } = "";
    public DateTime? FEC_REGISTRO            { get; set; }
    public string COD_USUARIO_ACTUALIZACION  { get; set; } = "";
    public string COD_ESTACION_ACTUALIZACION { get; set; } = "";
    public DateTime? FEC_ACTUALIZACION       { get; set; }
}
