namespace KONSolutions.Shared.Models.Estandar;

/// <summary>PROC_ESTANDAR_ENTIDADES_SHOWIMAGES — experimento: logos de todas las entidades
/// en un endpoint aparte del listado principal de Entidades (que no trae IMG_LOGO), para
/// que las grillas carguen el texto instantáneo y las imágenes lleguen después, en paralelo.</summary>
public class EntidadImagen
{
    public decimal? COD_ENTIDAD { get; set; }
    public byte[]? IMG_LOGO { get; set; }
}
