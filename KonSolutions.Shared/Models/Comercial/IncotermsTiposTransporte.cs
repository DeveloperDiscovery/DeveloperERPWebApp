using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Comercial;

public class IncotermsTiposTransporte
{
    [Required] public string COD_INCOTERM        { get; set; } = string.Empty;
    [Required] public string COD_TIPO_TRANSPORTE { get; set; } = string.Empty;
    public string COD_USUARIO_REGISTRO  { get; set; } = string.Empty;
    public string COD_ESTACION_REGISTRO { get; set; } = string.Empty;
    public DateTime? FEC_REGISTRO       { get; set; }

    // Solo lectura (vienen del JOIN).
    public string? DES_INCOTERM        { get; set; }
    public string? DES_TIPO_TRANSPORTE { get; set; }
}
