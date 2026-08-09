using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Comercial;

/// <summary>COMERCIAL.ENTIDADES_TEMPORADAS_COLECCIONES — Colecciones de una Temporada.
/// NUM_COLECCION NO es identity, lo ingresa el usuario. Hijo de EntidadesTemporadas.</summary>
public class EntidadesTemporadasColecciones
{
    public decimal? COD_ENTIDAD { get; set; }
    public int? NUM_TEMPORADA { get; set; }
    public int? NUM_COLECCION { get; set; }
    public string DES_COLECCION { get; set; } = string.Empty;
    public int? COD_TIPO_ESTADO { get; set; }
    public int? COD_ESTADO { get; set; }
    // Solo lectura (JOIN a COMERCIAL.ENTIDADES_TEMPORADAS).
    public string? DES_TEMPORADA { get; set; }
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

public class EntidadesTemporadasColeccionesInsertDto
{
    [Required] public decimal COD_ENTIDAD { get; set; }
    [Required] public int NUM_TEMPORADA { get; set; }
    [Required] public int NUM_COLECCION { get; set; }
    [Required] public string DES_COLECCION { get; set; } = string.Empty;
    public int? COD_TIPO_ESTADO { get; set; }
    public int? COD_ESTADO { get; set; }
}
