using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Comercial;

/// <summary>COMERCIAL.ENTIDADES_TEMPORADAS — Temporadas de una Entidad. NUM_TEMPORADA NO es
/// identity, lo ingresa el usuario. Padre de EntidadesTemporadasColecciones.</summary>
public class EntidadesTemporadas
{
    public decimal? COD_ENTIDAD { get; set; }
    public int? NUM_TEMPORADA { get; set; }
    public string DES_TEMPORADA { get; set; } = string.Empty;
    public int? NUM_ANO { get; set; }
    public int? NUM_MES { get; set; }
    public int? MES_FINAL { get; set; }
    public int? NUM_ORDEN_PRESENTACION { get; set; }
    public int? COD_TIPO_ESTADO { get; set; }
    public int? COD_ESTADO { get; set; }
    // Solo lectura (JOIN a ESTANDAR.ENTIDADES).
    public string? DES_COMERCIAL { get; set; }
    public string? DES_NOMBRE_COMPLETO { get; set; }
    // Solo lectura (JOIN a ESTANDAR.ESTADOS).
    public string? DES_BACKCOLOR { get; set; }
    public string? DES_ESTADO { get; set; }
    public string? DES_FORECOLOR { get; set; }
    public string COD_USUARIO_REGISTRO       { get; set; } = "";
    public string COD_ESTACION_REGISTRO      { get; set; } = "";
    public DateTime? FEC_REGISTRO            { get; set; }
    public string COD_USUARIO_ACTUALIZACION  { get; set; } = "";
    public string COD_ESTACION_ACTUALIZACION { get; set; } = "";
    public DateTime? FEC_ACTUALIZACION       { get; set; }
}

public class EntidadesTemporadasInsertDto
{
    [Required] public decimal COD_ENTIDAD { get; set; }
    public int? NUM_TEMPORADA { get; set; }
    [Required] public string DES_TEMPORADA { get; set; } = string.Empty;
    public int? NUM_ANO { get; set; }
    public int? NUM_MES { get; set; }
    public int? MES_FINAL { get; set; }
    public int? NUM_ORDEN_PRESENTACION { get; set; }
    public int? COD_TIPO_ESTADO { get; set; }
    public int? COD_ESTADO { get; set; }
}
