using System.ComponentModel.DataAnnotations;
namespace KONSolutions.Shared.Models.Logistica;

/// <summary>LOGISTICA.ALMACENES_TIPOS_MOVIMIENTO — qué tipos de movimiento están habilitados
/// en un almacén. Hija de LOGISTICA.ALMACENES.
/// PK compuesta: COD_ALMACEN + COD_CLASE_MOVIMIENTO + COD_TIPO_MOVIMIENTO.
///
/// No tiene datos propios más allá de la auditoría: por eso la pantalla no es un CRUD con
/// formulario, sino un asignar/desasignar entre dos listas.</summary>
public class AlmacenesTiposMovimiento
{
    [Required] public string COD_ALMACEN { get; set; } = "";
    [Required] public string COD_CLASE_MOVIMIENTO { get; set; } = "";
    [Required] public string COD_TIPO_MOVIMIENTO { get; set; } = "";

    public string COD_USUARIO_REGISTRO { get; set; } = "";
    public string COD_ESTACION_REGISTRO { get; set; } = "";
    public DateTime? FEC_REGISTRO { get; set; }

    // Solo lectura (JOIN con LOGISTICA.TIPOS_MOVIMIENTO).
    public string? DES_TIPO_MOVIMIENTO { get; set; }
    public string? DES_OBSERVACION { get; set; }
}

/// <summary>Asignación o baja de varios tipos en una sola llamada. Veinte tipos marcados no
/// deberían ser veinte peticiones: si una falla por el medio, el almacén queda a medio
/// asignar y sin forma de saber cuáles entraron.</summary>
public class AlmacenesTiposMovimientoLote
{
    [Required] public string COD_ALMACEN { get; set; } = "";
    [Required] public string COD_CLASE_MOVIMIENTO { get; set; } = "";
    public List<string> Tipos { get; set; } = new();
}

/// <summary>Qué pasó con el lote. Los fallidos se informan uno por uno: es normal que la baja
/// de algunos sea rechazada porque ya están en uso, y el usuario necesita saber cuáles.</summary>
public class AlmacenesTiposMovimientoLoteResult
{
    public int Procesados { get; set; }
    public List<string> Fallidos { get; set; } = new();
    public string? DesError { get; set; }
}
