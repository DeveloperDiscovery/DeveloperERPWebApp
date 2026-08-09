using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Entorno;

/// <summary>Catálogo ENTORNO.TIPOS_NOTIFICACION_ACCIONES — acciones disponibles por Tipo de
/// Notificación (ej. Aprobar/Rechazar/Observar/Marcar como leída).</summary>
public class TiposNotificacionAcciones
{
    public string COD_TIPO_NOTIFICACION { get; set; } = "";
    public int NUM_SECUENCIA { get; set; }
    public string? DES_ACCION { get; set; }
    public bool? FLG_LECTURA { get; set; }
    public bool? FLG_APROBAR { get; set; }
    public bool? FLG_RECHAZO { get; set; }
    public bool? FLG_OBSERVAR { get; set; }
}

/// <summary>DTO de guardado (PROC_ENTORNO_TIPOS_NOTIFICACION_ACCIONES_INSERT es upsert — siempre POST).</summary>
public class TiposNotificacionAccionesDto
{
    [Required] public string COD_TIPO_NOTIFICACION { get; set; } = "";
    // 0 al crear: el SP calcula la siguiente secuencia (MAX+1) por Tipo de Notificación.
    public int NUM_SECUENCIA { get; set; }
    [Required] public string DES_ACCION { get; set; } = "";
    public bool? FLG_LECTURA { get; set; }
    public bool? FLG_APROBAR { get; set; }
    public bool? FLG_RECHAZO { get; set; }
    public bool? FLG_OBSERVAR { get; set; }
}
