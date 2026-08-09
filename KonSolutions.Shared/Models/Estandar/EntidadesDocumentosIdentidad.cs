using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Estandar;

// ───────────── ESTANDAR.ENTIDADES_DOCUMENTOS_IDENTIDAD ─────────────
public class EntidadesDocumentosIdentidad
{
    public decimal? COD_ENTIDAD { get; set; }
    [Required] public int? COD_TIPO_DOCUMENTO_IDENTIDAD { get; set; }
    [Required] public string NUM_DOCUMENTO_IDENTIDAD { get; set; } = "";
    public DateTime? FEC_CADUCIDAD { get; set; }
    public string? COD_USUARIO_REGISTRO { get; set; }
    public string? COD_ESTACION_REGISTRO { get; set; }
    public DateTime? FEC_REGISTRO { get; set; }
    public string? COD_USUARIO_ACTUALIZACION { get; set; }
    public string? COD_ESTACION_ACTUALIZACION { get; set; }
    public DateTime? FEC_ACTUALIZACION { get; set; }
    // Desnormalizado del SP
    public string DES_TIPO_DOCUMENTO_IDENTIDAD { get; set; } = "";
}
