namespace KONSolutions.Shared.Models.Ingenieria;

/// <summary>Solo los campos de auditoría de INGENIERIA.ESPECIFICACIONES_TECNICAS_ITEMS —
/// se pide vía GetById puntual para alimentar PanelAuditoria en
/// EspecificacionesItemsTecnicosDialog.razor, cuyo árbol (treelist) no trae auditoría.</summary>
public class EspecificacionesTecnicasItemsAuditoria
{
    public string COD_USUARIO_REGISTRO { get; set; } = "";
    public string COD_ESTACION_REGISTRO { get; set; } = "";
    public DateTime? FEC_REGISTRO { get; set; }
    public string COD_USUARIO_ACTUALIZACION { get; set; } = "";
    public string COD_ESTACION_ACTUALIZACION { get; set; } = "";
    public DateTime? FEC_ACTUALIZACION { get; set; }
}
