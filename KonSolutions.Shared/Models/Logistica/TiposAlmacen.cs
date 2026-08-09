using System.ComponentModel.DataAnnotations;
namespace KONSolutions.Shared.Models.Logistica;

/// <summary>LOGISTICA.TIPOS_ALMACEN — catálogo de tipos de almacén. PK: COD_TIPO_ALMACEN.
/// Los FLG_* clasifican para qué sirve el almacén (regular, tránsito, producción,
/// producto terminado); no son excluyentes entre sí.</summary>
public class TiposAlmacen
{
    [Required] public string COD_TIPO_ALMACEN { get; set; } = "";
    [Required] public string DES_TIPO_ALMACEN { get; set; } = "";
    public bool? FLG_REGULAR { get; set; }
    public bool? FLG_TRANSITO { get; set; }
    public bool? FLG_PRODUCCION { get; set; }
    public bool? FLG_PRODUCTO_TERMINADO { get; set; }
}
