using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Sunat;

// ───────────── SUNAT.CUBSO_SUNAT ─────────────
public class CubsoSunat
{
    [Required] public string COD_CUBSO { get; set; } = "";
    public string? DES_CUBSO { get; set; }
    public string? DES_CUBSO_ES { get; set; }
    public string? COD_CUBSO_PARENT { get; set; }
    public string? COD_UNSPS { get; set; }
}
