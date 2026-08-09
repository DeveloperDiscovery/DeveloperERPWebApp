using System.ComponentModel.DataAnnotations;
namespace KONSolutions.Shared.Models.Zonificacion;

/// <summary>ZONIFICACION.TIPOS_ZONIFICACION — catálogo que clasifica las zonas de una
/// sucursal (piso, pasillo, sala, depósito…). PK: COD_TIPO_ZONIFICACION.
/// Sin auditoría ni estado. IMG_ICONO es el ícono con que se representa la zona.</summary>
public class TiposZonificacion
{
    [Required] public string COD_TIPO_ZONIFICACION { get; set; } = "";
    [Required] public string DES_TIPO_ZONIFICACION { get; set; } = "";
    public byte[]? IMG_ICONO { get; set; }
    /// <summary>Este tipo de zona se dibuja en el Mapa de Piso — en la práctica, los pisos.
    /// Se marca en el catálogo en vez de comparar contra un código fijo, que se rompería al
    /// cargar el catálogo en otra base.</summary>
    public bool? FLG_MAPEABLE { get; set; }
}
