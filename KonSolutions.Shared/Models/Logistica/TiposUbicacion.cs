using System.ComponentModel.DataAnnotations;
namespace KONSolutions.Shared.Models.Logistica;

/// <summary>LOGISTICA.TIPOS_UBICACION — catálogo que clasifica las ubicaciones de un
/// almacén (casillero de rack, zona de piso, playa de descarga…).
/// PK: COD_TIPO_UBICACION. Sin auditoría ni estado.</summary>
public class TiposUbicacion
{
    [Required] public string COD_TIPO_UBICACION { get; set; } = "";
    [Required] public string DES_TIPO_UBICACION { get; set; } = "";
    /// <summary>Imagen con que se representa el tipo de ubicación (columna IMG_IMAGEN).</summary>
    public byte[]? IMG_IMAGEN { get; set; }
}
