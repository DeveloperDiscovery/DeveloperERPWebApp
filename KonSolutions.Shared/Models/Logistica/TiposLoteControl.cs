using System.ComponentModel.DataAnnotations;
namespace KONSolutions.Shared.Models.Logistica;

/// <summary>LOGISTICA.TIPOS_LOTE_CONTROL</summary>
public class TiposLoteControl
{
    public string  COD_TIPO_LOTE_CONTROL { get; set; } = string.Empty;
    public string? DES_TIPO_LOTE_CONTROL { get; set; }
    public bool?   FLG_LOCAL             { get; set; }

    /// <summary>Sólo tiene sentido sobre un lote propio: uno de un tercero no puede ser
    /// interno.</summary>
    public bool?   FLG_INTERNO           { get; set; }
}
