using System.ComponentModel.DataAnnotations;
namespace KONSolutions.Shared.Models.Logistica;

/// <summary>LOGISTICA.LOTES_CONTROL</summary>
public class LotesControl
{
    public string    NUM_LOTE_CONTROL          { get; set; } = string.Empty;
    public string?   DES_LOTE_CONTROL          { get; set; }
    public DateTime? FEC_LOTE_CONTROL          { get; set; }
    public string?   COD_TIPO_LOTE_CONTROL     { get; set; }
    public decimal?  COD_ENTIDAD               { get; set; }
    public string?   NUM_LOTE_CONTROL_ENTIDAD  { get; set; }
    public decimal?  VAL_TOTAL                 { get; set; }
    public DateTime? FEC_VENCIMIENTO           { get; set; }
    public int?      COD_TIPO_ESTADO           { get; set; }
    public int?      COD_ESTADO                { get; set; }

    public string?   COD_USUARIO_REGISTRO      { get; set; }
    public string?   COD_ESTACION_REGISTRO     { get; set; }
    public DateTime? FEC_REGISTRO              { get; set; }
    public string?   COD_USUARIO_ACTUALIZACION { get; set; }
    public string?   COD_ESTACION_ACTUALIZACION{ get; set; }
    public DateTime? FEC_ACTUALIZACION         { get; set; }

    // Vienen resueltas por los JOIN del procedimiento: sólo se muestran.
    public string? DES_TIPO_LOTE_CONTROL { get; set; }
    public string? DES_ESTADO            { get; set; }
    public string? DES_BACKCOLOR         { get; set; }
    public string? DES_FORECOLOR         { get; set; }
    public string? DES_NOMBRE_COMPLETO   { get; set; }
    public string? DES_COMERCIAL         { get; set; }
    public string? DES_NOMBRE            { get; set; }
    public string? DES_NOMBRE2           { get; set; }
    public string? DES_PATERNO           { get; set; }
    public string? DES_MATERNO           { get; set; }
}
