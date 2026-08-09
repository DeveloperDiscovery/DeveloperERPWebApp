using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Ingenieria;

// ───────────── INGENIERIA.TIPOS_MERMA ─────────────
public class TiposMerma
{
    [Required] public string COD_TIPO_MERMA { get; set; } = "";
    [Required] public string DES_TIPO_MERMA { get; set; } = "";
    public bool? FLG_PRODUCCION { get; set; }
    public bool? FLG_LOGISTICA { get; set; }
    public byte[]? IMG_ICONO { get; set; }
}
