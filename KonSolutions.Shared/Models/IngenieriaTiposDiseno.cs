using System.ComponentModel.DataAnnotations;
namespace KONSolutions.Shared.Models.Ingenieria;

/// <summary>Entidad TIPOS_DISENO - tipos equivalentes a la BD.</summary>
public class TiposDiseno
{
    [Required] public string COD_TIPO_DISENO { get; set; } = "";
    [Required] public string DES_TIPO_DISENO { get; set; } = "";
    public bool? FLG_PROPIO { get; set; }
    public bool? FLG_PRODUCCION { get; set; }
    public bool? FLG_MUESTRA { get; set; }
    public bool? FLG_COTIZACION { get; set; }
    public byte? NUM_ORDEN_PRESENTACION { get; set; }
    public string COD_USUARIO_REGISTRO       { get; set; } = "";
    public string COD_ESTACION_REGISTRO      { get; set; } = "";
    public DateTime? FEC_REGISTRO            { get; set; }
    public string COD_USUARIO_ACTUALIZACION  { get; set; } = "";
    public string COD_ESTACION_ACTUALIZACION { get; set; } = "";
    public DateTime? FEC_ACTUALIZACION       { get; set; }
}
