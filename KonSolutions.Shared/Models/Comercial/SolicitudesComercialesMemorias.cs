using System.ComponentModel.DataAnnotations;
namespace KONSolutions.Shared.Models.Comercial;

/// <summary>COMERCIAL.SOLICITUDES_COMERCIALES_MEMORIAS — bitácora de anotaciones libres por
/// solicitud comercial ("Ayuda Memoria"). PK: NUM_SOLICITUD_COMERCIAL + NUM_SECUENCIA.</summary>
public class SolicitudesComercialesMemorias
{
    public int NUM_SOLICITUD_COMERCIAL { get; set; }
    public short NUM_SECUENCIA { get; set; }
    public string? DES_MEMORIA { get; set; }
    public string? COD_USUARIO_REGISTRO { get; set; }
    public string? COD_ESTACION_REGISTRO { get; set; }
    public DateTime? FEC_REGISTRO { get; set; }
    public string? COD_USUARIO_ACTUALIZACION { get; set; }
    public string? COD_ESTACION_ACTUALIZACION { get; set; }
    public DateTime? FEC_ACTUALIZACION { get; set; }
}

public class SolicitudesComercialesMemoriasInsertDto
{
    [Required] public int NUM_SOLICITUD_COMERCIAL { get; set; }
    [Required] public short NUM_SECUENCIA { get; set; }
    [Required] public string DES_MEMORIA { get; set; } = string.Empty;
}
