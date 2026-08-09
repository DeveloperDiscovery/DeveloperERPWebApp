using System.ComponentModel.DataAnnotations;
namespace KONSolutions.Shared.Models.Logistica;

/// <summary>LOGISTICA.CONTENEDORES_ALMACENAJE — el mueble físico que ocupa un espacio del
/// almacén (rack, góndola, jaula…). PK: COD_CONTENEDOR.
///
/// Lleva la geometría (DES_POLIGONO) y la grilla (NUM_FILAS × NUM_COLUMNAS) desde las que
/// se derivan sus casilleros: el polígono se dibuja UNA vez sobre el plano del nivel y la
/// posición de cada casillero se calcula, no se captura.
///
/// OJO con DES_CONTENDOR: así se llama la columna en la BD (le falta una "O"). El nombre
/// de la propiedad la respeta a propósito para que el mapeo funcione sin alias.</summary>
public class ContenedoresAlmacenaje
{
    // Sin [Required]: al Crear va vacío a propósito — el SP genera el correlativo.
    public string COD_CONTENEDOR { get; set; } = "";
    [Required] public string DES_CONTENDOR { get; set; } = "";
    public string? COD_TIPO_UBICACION { get; set; }
    public string? COD_SUCURSAL { get; set; }
    public string? COD_ZONIFICACION { get; set; }
    public string? COD_ALMACEN { get; set; }
    // Polígono del contenedor sobre el plano del nivel. La UI no lo edita a mano en el CRUD,
    // pero SÍ lo transporta en el POST para no borrarlo al guardar desde el formulario.
    public string? DES_POLIGONO { get; set; }
    public int? NUM_FILAS { get; set; }
    public int? NUM_COLUMNAS { get; set; }
    public int? VAL_ALTO_NIVEL_CENTIMETROS { get; set; }
    public int? VAL_LARGO_NIVEL_CENTIMETROS { get; set; }
    public int? VAL_PROFUNDIDAD_NIVEL_CENTIMETROS { get; set; }
    public int? COD_TIPO_ESTADO { get; set; }
    public int? COD_ESTADO { get; set; }

    // Auditoría: la escribe el SP con el usuario/estación del token. Se lee para el
    // PanelAuditoria del diálogo.
    public string COD_USUARIO_REGISTRO { get; set; } = "";
    public string COD_ESTACION_REGISTRO { get; set; } = "";
    public DateTime? FEC_REGISTRO { get; set; }
    public string COD_USUARIO_ACTUALIZACION { get; set; } = "";
    public string COD_ESTACION_ACTUALIZACION { get; set; } = "";
    public DateTime? FEC_ACTUALIZACION { get; set; }

    // Solo lectura (JOINs).
    public string? DES_TIPO_UBICACION { get; set; }
    public string? DES_ALMACEN { get; set; }
    public string? DES_ABREVIADA { get; set; }
    public string? DES_SUCURSAL { get; set; }
    public string? DES_ZONIFICACION { get; set; }
    /// <summary>Contorno del ALMACÉN al que pertenece, ya con su nombre propio en el SP
    /// (c.[DES_POLIGONO] AS [DES_POLIGONO_ALMACENES]). Antes venía con el mismo nombre que el
    /// del contenedor y lo pisaba; el sufijo con la tabla de origen es lo que evita que dos
    /// columnas homónimas compitan por la misma propiedad.</summary>
    public string? DES_POLIGONO_ALMACENES { get; set; }
    public string? DES_ESTADO { get; set; }
    public string? DES_BACKCOLOR { get; set; }
    public string? DES_FORECOLOR { get; set; }

    /// <summary>Total de casilleros que genera la grilla del contenedor.</summary>
    public int TotalCasilleros => (NUM_FILAS ?? 0) * (NUM_COLUMNAS ?? 0);
}
