using System.ComponentModel.DataAnnotations;
namespace KONSolutions.Shared.Models.Zonificacion;

/// <summary>ZONIFICACION.ZONIFICACION — zonas de una sucursal, jerárquicas
/// (COD_ZONIFICACION_PADRE) y por nivel/piso (NUM_NIVEL).
/// PK compuesta: COD_SUCURSAL + COD_ZONIFICACION.
/// IMG_PLANO guarda el plano del nivel y VAL_ANCHO_METROS/VAL_LARGO_METROS su escala real:
/// juntos permiten ubicar geométricamente lo que se dibuje encima.</summary>
public class Zonificacion
{
    [Required] public string COD_SUCURSAL { get; set; } = "";
    // Sin [Required]: al Crear va vacío a propósito — el SP arma el código como
    // COD_ZONIFICACION_PADRE + correlativo de 3 dígitos.
    public string COD_ZONIFICACION { get; set; } = "";
    [Required] public string DES_ZONIFICACION { get; set; } = "";
    public string? COD_ZONIFICACION_PADRE { get; set; }
    public string? COD_TIPO_ZONIFICACION { get; set; }
    public int? NUM_NIVEL { get; set; }
    public int? NUM_NIVEL_ORDEN { get; set; }
    public int? VAL_ANCHO_METROS { get; set; }
    public int? VAL_LARGO_METROS { get; set; }
    public bool? FLG_MOSTRAR_DESCENDENTE { get; set; }
    public byte[]? IMG_PLANO { get; set; }
    public int? COD_TIPO_ESTADO { get; set; }
    public int? COD_ESTADO { get; set; }

    // Solo lectura (JOINs).
    public string? DES_TIPO_ZONIFICACION { get; set; }
    public string? DES_SUCURSAL { get; set; }
    public string? DES_ESTADO { get; set; }
    public string? DES_BACKCOLOR { get; set; }
    public string? DES_FORECOLOR { get; set; }
}
