using System.ComponentModel.DataAnnotations;
namespace KONSolutions.Shared.Models.Logistica;

/// <summary>LOGISTICA.TIPOS_REQUERIMIENTO</summary>
public class TiposRequerimiento
{
    public string COD_TIPO_REQUERIMIENTO { get; set; } = string.Empty;
    public string DES_TIPO_REQUERIMIENTO { get; set; } = string.Empty;
    public bool? FLG_TRANSFERENCIA { get; set; }
    public bool? FLG_PRODUCCION { get; set; }
    public bool? FLG_COMPRA { get; set; }
    public bool? FLG_CONSUMO_INTERNO { get; set; }
    public bool? FLG_REPOSICION { get; set; }
}
