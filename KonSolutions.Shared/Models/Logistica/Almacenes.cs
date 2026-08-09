using System.ComponentModel.DataAnnotations;
namespace KONSolutions.Shared.Models.Logistica;

/// <summary>LOGISTICA.ALMACENES — catálogo de almacenes. PK: COD_ALMACEN.
/// Los campos DES_* al final son de solo lectura: los resuelven los JOIN de los SP.</summary>
public class Almacenes
{
    // Sin [Required]: al Crear va vacío a propósito — el SP genera el correlativo
    // (PROC_LOGISTICA_ALMACENES_INSERT: MAX + FNC_ObtenerSiguienteCorrelativo).
    public string COD_ALMACEN { get; set; } = "";
    [Required] public string DES_ALMACEN { get; set; } = "";
    public string? DES_ABREVIADA { get; set; }
    public string? COD_TIPO_ALMACEN { get; set; }
    public string? COD_SUCURSAL { get; set; }
    // Zona de la sucursal donde vive el almacén: junto con DES_POLIGONO permite ubicarlo
    // sobre el plano del nivel (ZONIFICACION.IMG_PLANO).
    public string? COD_ZONIFICACION { get; set; }
    public bool? FLG_STOCK_NEGATIVO { get; set; }
    public decimal? COD_ENTIDAD_PROPIETARIA { get; set; }
    public string? COD_PROCESO { get; set; }
    public string? COD_SUB_PROCESO { get; set; }
    // Polígono del almacén sobre el plano. Texto libre (nvarchar(max)); la UI no lo edita
    // a mano, pero SÍ lo transporta en el POST para no borrarlo al guardar desde el CRUD.
    public string? DES_POLIGONO { get; set; }
    public int? COD_TIPO_ESTADO { get; set; }
    public int? COD_ESTADO { get; set; }
    public string COD_USUARIO_REGISTRO { get; set; } = "";
    public string COD_ESTACION_REGISTRO { get; set; } = "";
    public DateTime? FEC_REGISTRO { get; set; }
    public string COD_USUARIO_ACTUALIZACION { get; set; } = "";
    public string COD_ESTACION_ACTUALIZACION { get; set; } = "";
    public DateTime? FEC_ACTUALIZACION { get; set; }

    // Solo lectura (JOINs).
    public string? DES_TIPO_ALMACEN { get; set; }
    public string? DES_SUCURSAL { get; set; }
    public string? DES_ZONIFICACION { get; set; }
    public string? DES_SUB_PROCESO { get; set; }
    public string? DES_NOMBRE_COMPLETO { get; set; }
    public string? DES_BACKCOLOR { get; set; }
    public string? DES_ESTADO { get; set; }
    public string? DES_FORECOLOR { get; set; }
    // Alias del estado: 'ANU' deja el almacén en solo lectura (ver EstadoAlias).
    public string? COD_ESTADO_ALIAS { get; set; }
}
