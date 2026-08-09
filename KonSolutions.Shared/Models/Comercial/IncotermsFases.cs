using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Comercial;

public class IncotermsFases
{
    [Required] public string COD_INCOTERM      { get; set; } = string.Empty;
    [Required] public string COD_FASE_INCOTERM { get; set; } = string.Empty;
    public bool? FLG_VENDEDOR             { get; set; }
    public int?  NUM_ORDEN_PRESENTACION   { get; set; }

    // Solo lectura (vienen del JOIN).
    public string? DES_FASE_INCOTERM { get; set; }
    public string? DES_INCOTERM      { get; set; }
}
