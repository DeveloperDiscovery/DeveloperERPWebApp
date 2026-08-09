using System.ComponentModel.DataAnnotations;
namespace KONSolutions.Shared.Models.Logistica;

/// <summary>LOGISTICA.CONTENEDORES_ALMACENAJE_CASILLEROS — cada celda de la grilla de un
/// contenedor. Hija de LOGISTICA.CONTENEDORES_ALMACENAJE.
/// PK compuesta: COD_CONTENEDOR + COD_CASILLERO.
///
/// El casillero es el mueble; la UBICACIÓN (COD_ALMACEN + COD_UBICACION_ALMACEN) es la
/// dirección a la que apuntan stocks y movimientos. Normalmente los crea el generador
/// (_CREATETALL), que además da de alta una ubicación por casillero.
///
/// No tiene columnas de actualización: el SP solo escribe auditoría de registro, así que
/// el PanelAuditoria de esta entidad muestra únicamente el alta.</summary>
public class ContenedoresAlmacenajeCasilleros
{
    [Required] public string COD_CONTENEDOR { get; set; } = "";
    // Sin [Required]: al Crear va vacío — el SP genera el correlativo dentro del contenedor.
    public string COD_CASILLERO { get; set; } = "";
    [Required] public string DES_CASILLERO { get; set; } = "";
    /// <summary>Nivel (altura) dentro del contenedor.</summary>
    public int? NUM_FILA { get; set; }
    /// <summary>Posición a lo largo del contenedor.</summary>
    public int? NUM_COLUMNA { get; set; }
    public string? COD_ALMACEN { get; set; }
    public string? COD_UBICACION_ALMACEN { get; set; }

    // Auditoría: la escribe el SP con el usuario/estación del token.
    public string COD_USUARIO_REGISTRO { get; set; } = "";
    public string COD_ESTACION_REGISTRO { get; set; } = "";
    public DateTime? FEC_REGISTRO { get; set; }

    // Solo lectura (JOINs).
    public string? DES_CONTENDOR { get; set; }   // sic: así se llama la columna en la BD
    public string? DES_ALMACEN { get; set; }
    public string? DES_ABREVIADA { get; set; }
    public string? DES_UBICACION_ALMACEN { get; set; }
    /// <summary>Polígono de la UBICACIÓN a la que apunta el casillero. Es la única de las
    /// tres columnas homónimas que conserva el nombre simple en el SP, porque la tabla base
    /// no tiene DES_POLIGONO propio.</summary>
    public string? DES_POLIGONO { get; set; }

    /// <summary>Polígono del CONTENEDOR: el rectángulo del mueble sobre el plano. Es el que
    /// sirve para derivar la geometría del casillero.</summary>
    public string? DES_POLIGONO_CONTENEDORES_ALMACENAJE { get; set; }

    /// <summary>Contorno del ALMACÉN.</summary>
    public string? DES_POLIGONO_ALMACENES { get; set; }

    /// <summary>Etiqueta corta de posición para la grilla: F1·C3.</summary>
    public string Posicion =>
        NUM_FILA is null && NUM_COLUMNA is null ? "" : $"F{NUM_FILA}·C{NUM_COLUMNA}";

    /// <summary>Un casillero sin ubicación no direcciona nada: el stock no tendría dónde
    /// apoyarse. Se marca en la grilla para que se note.</summary>
    public bool SinUbicacion => string.IsNullOrWhiteSpace(COD_UBICACION_ALMACEN);
}

/// <summary>Parámetros del generador (_CREATETALL). Es destructivo: reemplaza los casilleros
/// del contenedor por una matriz nueva de FILAS × COLUMNAS.</summary>
public class ContenedoresAlmacenajeCasillerosGenerar
{
    [Required] public string COD_CONTENEDOR { get; set; } = "";
    [Required] public string COD_ALMACEN { get; set; } = "";
    public int NUM_FILAS { get; set; }
    public int NUM_COLUMNAS { get; set; }
}
