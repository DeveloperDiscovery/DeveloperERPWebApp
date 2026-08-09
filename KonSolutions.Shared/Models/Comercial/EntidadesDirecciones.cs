using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Comercial;

// ───────────── COMERCIAL.ENTIDADES_DIRECCIONES ─────────────
public class EntidadesDirecciones
{
    public decimal? COD_ENTIDAD { get; set; }
    public int? NUM_SECUENCIA { get; set; }
    public int? COD_TIPO_DIRECCION { get; set; }
    [Required] public string? COD_UBIGEO { get; set; }
    [Required] public string? COD_TIPO_ZONA { get; set; }
    [Required] public string? DES_NOMBRE_ZONA { get; set; }
    [Required] public string? COD_TIPO_VIA { get; set; }
    [Required] public string? DES_NOMBRE_VIA { get; set; }
    [Required] public string? DES_NUMERACION { get; set; }
    public string? DES_KILOMETRO { get; set; }
    public string? DES_MANZANA { get; set; }
    public string? DES_LOTE { get; set; }
    public string? DES_INTERIOR { get; set; }
    [Required] public string? DES_DIRECCION { get; set; }
    public string? DES_REFERENCIA { get; set; }
    public decimal? VAL_LATITUD { get; set; }
    public decimal? VAL_LONGITUD { get; set; }
    public int? COD_TIPO_ESTADO { get; set; }
    public int? COD_ESTADO { get; set; }
    public string? COD_USUARIO_REGISTRO { get; set; }
    public string? COD_ESTACION_REGISTRO { get; set; }
    public DateTime? FEC_REGISTRO { get; set; }
    public string? COD_USUARIO_ACTUALIZACION { get; set; }
    public string? COD_ESTACION_ACTUALIZACION { get; set; }
    public DateTime? FEC_ACTUALIZACION { get; set; }
    // Desnormalizados del SP
    public string DES_TIPO_DIRECCION { get; set; } = "";
    public string DES_TIPO_VIA { get; set; } = "";
    public string DES_TIPO_ZONA { get; set; } = "";
    public string DES_UBIGEO { get; set; } = "";
    public string DES_ESTADO { get; set; } = "";
    public string DES_BACKCOLOR { get; set; } = "";
    public string DES_FORECOLOR { get; set; } = "";
}
