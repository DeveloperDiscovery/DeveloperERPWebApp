using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Comercial;

/// <summary>Catálogo COMERCIAL.TIPOS_VALOR_CALCULO. Solo COD_TIPO_VALOR_CALCULO es
/// NOT NULL (PK); el resto admite null en la tabla y no es obligatorio en el form.</summary>
public class TiposValorCalculo
{
    [Required] public string COD_TIPO_VALOR_CALCULO { get; set; } = string.Empty;
    public string? DES_TIPO_VALOR_CALCULO { get; set; }
    public string? COD_TIPO_IMPUESTO      { get; set; }
    public bool?   FLG_POSITIVO           { get; set; }
    public bool?   FLG_IMPUESTO           { get; set; }
    public bool?   FLG_CALCULA            { get; set; }
    public bool?   FLG_REGIMEN_TRIBUTARIO { get; set; }
    public string? DES_ETIQUETA_XML       { get; set; }

    // Solo lectura (viene del JOIN con TIPOS_IMPUESTO en el SHOWALL/SEARCH).
    public string? DES_TIPO_IMPUESTO      { get; set; }
}
