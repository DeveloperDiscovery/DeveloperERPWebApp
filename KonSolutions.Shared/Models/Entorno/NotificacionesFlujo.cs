namespace KONSolutions.Shared.Models.Entorno;

public class NotificacionFlujoItem
{
    public int     NUM_NOTIFICACION       { get; set; }
    public string? DES_NOTIFICACION       { get; set; }
    public string? DES_INFORMACION        { get; set; }
    public string  COD_TIPO_NOTIFICACION  { get; set; } = string.Empty;
    public string? DES_TIPO_NOTIFICACION  { get; set; }
    public byte[]? IMG_ICONO              { get; set; }
    public bool?   FLG_APROBACION         { get; set; }
    public int?    DES_TIEMPO_RESTANTE    { get; set; }
    public string? COD_USUARIO_NOTIFICADOR { get; set; }
    public string? DES_USUARIO            { get; set; }
}

public class NotificacionResumenTipo
{
    public string  COD_TIPO_NOTIFICACION  { get; set; } = string.Empty;
    public string? DES_TIPO_NOTIFICACION  { get; set; }
    public int     NUM_NOTIFICACIONES     { get; set; }
    public byte[]? IMG_ICONO              { get; set; }
    public bool?   FLG_APROBACION         { get; set; }
}

public class NotificacionesFlujoResult
{
    public List<NotificacionFlujoItem>   Notificaciones { get; set; } = new();
    public List<NotificacionResumenTipo> Resumen        { get; set; } = new();
}

/// <summary>Fila del listado Recibidas/Emitidas (PROC_ENTORNO_NOTIFICACIONES_FLUJO_SHOWALL).</summary>
public class NotificacionFlujoDetalle
{
    public int      NUM_NOTIFICACION          { get; set; }
    public string?  DES_NOTIFICACION          { get; set; }
    public string?  DES_INFORMACION           { get; set; }
    public byte[]?  IMG_TIPO_NOTIFICACION     { get; set; }
    public string?  COD_TIPO_NOTIFICACION     { get; set; }
    public string?  DES_TIPO_NOTIFICACION     { get; set; }
    public string?  COD_USUARIO               { get; set; }
    public string?  DES_USUARIO               { get; set; }
    public string?  DES_CORREO_INSTITUCIONAL  { get; set; }
    public string?  NUM_TELEFONO              { get; set; }
    public DateTime? FEC_NOTIFICACION         { get; set; }
    public DateTime? FEC_VENCIMIENTO          { get; set; }
    public string?  DES_ESTADO_FLUJO          { get; set; }
    public byte[]?  IMG_PICTURE_FLLUJO        { get; set; }
    public string?  DES_ESTADO_NOTIFICACION   { get; set; }
    public byte[]?  IMG_PICTURE_NOTIFICACION  { get; set; }
}
