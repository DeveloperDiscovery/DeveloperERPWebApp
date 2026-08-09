using System.ComponentModel.DataAnnotations;
namespace KONSolutions.Shared.Models.Logistica;

/// <summary>LOGISTICA.ALMACENES_UBICACIONES_ALMACEN — la DIRECCIÓN dentro de un almacén.
/// Hija de LOGISTICA.ALMACENES. PK compuesta: COD_ALMACEN + COD_UBICACION_ALMACEN.
///
/// Es el direccionamiento genérico del sistema: stocks y movimientos apuntan acá. El mueble
/// que la ocupa (rack, góndola…) vive en CONTENEDORES_ALMACENAJE_CASILLEROS y referencia
/// esta ubicación, no al revés — por eso rack/fila/columna ya no viven acá.</summary>
public class AlmacenesUbicacionesAlmacen
{
    [Required] public string COD_ALMACEN { get; set; } = "";
    // El SP ya no genera correlativo: el código lo trae quien crea la ubicación.
    [Required] public string COD_UBICACION_ALMACEN { get; set; } = "";
    [Required] public string DES_UBICACION_ALMACEN { get; set; } = "";
    /// <summary>Polígono propio sobre el plano del nivel. En las ubicaciones de casillero
    /// queda NULL: su geometría se deriva del polígono del contenedor.</summary>
    public string? DES_POLIGONO { get; set; }
    /// <summary>La creó el generador de casilleros, no una persona. Lo escribe el SP; la UI
    /// solo lo muestra — nunca es editable.</summary>
    public bool? FLG_CREACION_AUTOMATICA { get; set; }

    // Auditoría: la escribe el SP con el usuario/estación del token.
    public string COD_USUARIO_REGISTRO { get; set; } = "";
    public string COD_ESTACION_REGISTRO { get; set; } = "";
    public DateTime? FEC_REGISTRO { get; set; }
    public string COD_USUARIO_ACTUALIZACION { get; set; } = "";
    public string COD_ESTACION_ACTUALIZACION { get; set; } = "";
    public DateTime? FEC_ACTUALIZACION { get; set; }

    // Solo lectura (JOIN con LOGISTICA.ALMACENES).
    public string? DES_ABREVIADA { get; set; }
    public string? DES_ALMACEN { get; set; }
    /// <summary>Casillero que ocupa esta ubicación (JOIN con
    /// LOGISTICA.CONTENEDORES_ALMACENAJE_CASILLEROS). Vacío cuando la ubicación está libre,
    /// es decir, cuando ningún mueble la está usando todavía.</summary>
    public string? DES_CASILLERO { get; set; }
    /// <summary>Contorno del ALMACÉN padre, ya con su nombre propio en el SP
    /// (b.[DES_POLIGONO] AS [DES_POLIGONO_ALMACENES]).</summary>
    public string? DES_POLIGONO_ALMACENES { get; set; }
}
