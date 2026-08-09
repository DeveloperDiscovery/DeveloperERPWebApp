using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Comercial;

// ───────────── COMERCIAL.ENTIDADES_TIPOS_MEDIO_CONTACTO ─────────────
// Clave: COD_ENTIDAD + NUM_SECUENCIA (auto-generado por el SP INSERT) — permite
// varias filas del mismo COD_TIPO_MEDIO_CONTACTO para una misma entidad.
public class EntidadesTiposMedioContacto
{
    public decimal? COD_ENTIDAD { get; set; }
    public short? NUM_SECUENCIA { get; set; }
    [Required] public int? COD_TIPO_MEDIO_CONTACTO { get; set; }
    [Required] public string DAT_TIPO_MEDIO_CONTACTO { get; set; } = "";
    public string? COD_TIPO_USO { get; set; }
    public string? COD_USUARIO_REGISTRO { get; set; }
    public string? COD_ESTACION_REGISTRO { get; set; }
    public DateTime? FEC_REGISTRO { get; set; }
    public string? COD_USUARIO_ACTUALIZACION { get; set; }
    public string? COD_ESTACION_ACTUALIZACION { get; set; }
    public DateTime? FEC_ACTUALIZACION { get; set; }
    // Desnormalizado del SP
    public string DES_TIPO_MEDIO_CONTACTO { get; set; } = "";
    public string DES_TIPO_USO { get; set; } = "";
}
