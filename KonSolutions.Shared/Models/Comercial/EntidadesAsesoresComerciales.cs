using System.ComponentModel.DataAnnotations;
namespace KONSolutions.Shared.Models.Comercial;

/// <summary>COMERCIAL.ENTIDADES_ASESORES_COMERCIALES — asesores comerciales asignados a un
/// cliente (PK COD_ENTIDAD_CLIENTE + COD_ENTIDAD_ASESOR_COMERCIAL). Solo aplica al Catálogo
/// de Clientes (COD_TIPO_ENTIDAD = 'CLI') — ver tab "Asesores Comerciales" en EntidadesDialog.</summary>
public class EntidadesAsesoresComerciales
{
    public decimal? COD_ENTIDAD_CLIENTE { get; set; }
    public decimal? COD_ENTIDAD_ASESOR_COMERCIAL { get; set; }
    public string? COD_USUARIO_REGISTRO { get; set; }
    public string? COD_ESTACION_REGISTRO { get; set; }
    public DateTime? FEC_REGISTRO { get; set; }
    public string? DES_NOMBRE_COMPLETO { get; set; }
    public string? DES_COMERCIAL { get; set; }
}

public class EntidadesAsesoresComercialesInsertDto
{
    [Required] public decimal? COD_ENTIDAD_CLIENTE { get; set; }
    [Required] public decimal? COD_ENTIDAD_ASESOR_COMERCIAL { get; set; }
}
