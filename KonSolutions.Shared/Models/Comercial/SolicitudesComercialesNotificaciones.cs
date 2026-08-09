namespace KONSolutions.Shared.Models.Comercial;

/// <summary>COMERCIAL.SOLICITUDES_COMERCIALES_NOTIFICACIONES — notificaciones generadas por
/// una solicitud comercial. Solo lectura desde la UI ("Ver Aprobaciones"): las filas las
/// crea el flujo de notificación, no el usuario. Los SP ya traen resueltos el tipo, estado
/// y colores desde ENTORNO.NOTIFICACIONES / TIPOS_NOTIFICACION / ESTANDAR.ESTADOS.</summary>
public class SolicitudesComercialesNotificaciones
{
    public int NUM_SOLICITUD_COMERCIAL { get; set; }
    public int NUM_NOTIFICACION { get; set; }
    public string? DES_TIPO_NOTIFICACION { get; set; }
    public string? DES_INFORMACION { get; set; }
    public string? DES_NOTIFICACION { get; set; }
    public string? DES_OBSERVACIONES { get; set; }
    public short? COD_ESTADO { get; set; }
    public string? DES_ESTADO { get; set; }
    public string? DES_BACKCOLOR { get; set; }
    public string? DES_FORECOLOR { get; set; }
    public byte[]? IMG_PICTURE { get; set; }
    public string? COD_USUARIO_REGISTRO { get; set; }
    public string? COD_ESTACION_REGISTRO { get; set; }
    public DateTime? FEC_REGISTRO { get; set; }
}
