namespace KONSolutions.Shared.Models.Entorno;

// Espejo cliente de los resultados del Dashboard de Monitoreo de BD.
// Coincide 1:1 con los modelos del API (KonSolutionsApi.Domain.Models.Entorno.DashboardBd*).

public class DashboardBdResumen
{
    public string?   DES_BASE_DATOS          { get; set; }
    public string?   DES_VERSION_SQL         { get; set; }
    public string?   DES_EDICION_SQL         { get; set; }
    public int?      DES_NIVEL_COMPAT        { get; set; }
    public string?   DES_MODELO_RECUPERACION { get; set; }
    public string?   DES_ESTADO              { get; set; }
    public decimal?  VAL_TAMANO_DATOS_MB     { get; set; }
    public decimal?  VAL_DATOS_USADOS_MB     { get; set; }
    public decimal?  VAL_TAMANO_LOG_MB       { get; set; }
    public decimal?  VAL_LOG_USADO_MB        { get; set; }
    public decimal?  VAL_TAMANO_TOTAL_MB     { get; set; }
    public int       NUM_CONEXIONES          { get; set; }
    public int       NUM_SESIONES_ACTIVAS    { get; set; }
    public int       NUM_BLOQUEOS            { get; set; }
    public DateTime? FEC_INICIO_SQL          { get; set; }
    public int       NUM_DIAS_ACTIVO         { get; set; }
}

public class DashboardBdSesion
{
    public int      NUM_SESION           { get; set; }
    public string?  DES_ESTADO           { get; set; }
    public string?  DES_LOGIN            { get; set; }
    public string?  DES_HOST             { get; set; }
    public string?  DES_PROGRAMA         { get; set; }
    public int      NUM_BLOQUEADO_POR    { get; set; }
    public string?  DES_TIPO_ESPERA      { get; set; }
    public long     VAL_TIEMPO_ESPERA_MS { get; set; }
    public long     VAL_DURACION_MS      { get; set; }
    public long     VAL_CPU_MS           { get; set; }
    public long     VAL_LECTURAS         { get; set; }
    public string?  DES_COMANDO          { get; set; }
    public string?  DES_CONSULTA         { get; set; }
}

public class DashboardBdBloqueo
{
    public int     NUM_SESION_BLOQUEADA   { get; set; }
    public int     NUM_SESION_BLOQUEANTE  { get; set; }
    public string? DES_TIPO_ESPERA        { get; set; }
    public long    VAL_TIEMPO_ESPERA_MS   { get; set; }
    public string? DES_RECURSO            { get; set; }
    public string? DES_LOGIN_BLOQUEADA    { get; set; }
    public string? DES_LOGIN_BLOQUEANTE   { get; set; }
    public string? DES_CONSULTA_BLOQUEADA { get; set; }
}

public class DashboardBdConsulta
{
    public long     NUM_EJECUCIONES      { get; set; }
    public decimal  VAL_DURACION_PROM_MS { get; set; }
    public decimal  VAL_CPU_PROM_MS      { get; set; }
    public long     VAL_LECTURAS_PROM    { get; set; }
    public decimal  VAL_DURACION_TOT_MS  { get; set; }
    public DateTime? FEC_ULTIMA_EJECUCION { get; set; }
    public string?  DES_CONSULTA         { get; set; }
}

public class DashboardBdWait
{
    public string?  DES_TIPO_ESPERA { get; set; }
    public long     VAL_TIEMPO_MS   { get; set; }
    public decimal  VAL_PORCENTAJE  { get; set; }
}

public class DashboardBdTabla
{
    public string?  DES_SCHEMA   { get; set; }
    public string?  DES_TABLA    { get; set; }
    public long     NUM_FILAS    { get; set; }
    public decimal  VAL_TOTAL_MB { get; set; }
    public decimal  VAL_USADO_MB { get; set; }
    public decimal  VAL_DATOS_MB { get; set; }
}

public class DashboardBdFragmentacion
{
    public string?  DES_SCHEMA        { get; set; }
    public string?  DES_TABLA         { get; set; }
    public string?  DES_INDICE        { get; set; }
    public string?  DES_TIPO_INDICE   { get; set; }
    public decimal  VAL_FRAGMENTACION { get; set; }
    public long     NUM_PAGINAS       { get; set; }
    public string?  DES_ACCION        { get; set; }
}

public class DashboardBdIndiceFaltante
{
    public string?  DES_TABLA         { get; set; }
    public decimal  VAL_IMPACTO       { get; set; }
    public long     NUM_BUSQUEDAS     { get; set; }
    public long     NUM_ESCANEOS      { get; set; }
    public string?  DES_COLUMNAS_EQ   { get; set; }
    public string?  DES_COLUMNAS_INEQ { get; set; }
    public string?  DES_COLUMNAS_INCL { get; set; }
    public decimal  VAL_COSTO_PROM    { get; set; }
}

public class DashboardBdArchivo
{
    public string?  DES_ARCHIVO       { get; set; }
    public string?  DES_TIPO          { get; set; }
    public string?  DES_RUTA          { get; set; }
    public decimal  VAL_TAMANO_MB     { get; set; }
    public decimal  VAL_USADO_MB      { get; set; }
    public decimal  VAL_LIBRE_MB      { get; set; }
    public decimal  VAL_PCT_USADO     { get; set; }
    public string?  DES_CRECIMIENTO   { get; set; }
    public decimal? VAL_MAX_TAMANO_MB { get; set; }
}

public class DashboardBdBackup
{
    public string?   DES_TIPO_BACKUP { get; set; }
    public DateTime? FEC_ULTIMO      { get; set; }
    public decimal   VAL_TAMANO_MB   { get; set; }
    public int       NUM_DIAS_DESDE  { get; set; }
}

public class DashboardBdMemoria
{
    public long?    VAL_MEMORIA_TOTAL_MB { get; set; }
    public long?    VAL_MEMORIA_LIBRE_MB { get; set; }
    public long?    VAL_SQL_USANDO_MB    { get; set; }
    public long?    VAL_PAGE_LIFE_EXPECT { get; set; }
    public decimal? VAL_BUFFER_CACHE_HIT { get; set; }
    public decimal? VAL_BUFFER_BD_MB     { get; set; }
}

public class DashboardBdLimpiezaLog
{
    public decimal VAL_ANTES_MB    { get; set; }
    public decimal VAL_DESPUES_MB  { get; set; }
    public decimal VAL_LIBERADO_MB { get; set; }
}

public class DashboardBdBackupResultado
{
    public string?  DES_ARCHIVO   { get; set; }
    public string?  DES_TIPO      { get; set; }
    public int      VAL_SEGUNDOS  { get; set; }
    public decimal? VAL_TAMANO_MB { get; set; }
}

public class DashboardBdKillResultado
{
    public int     NUM_SESION    { get; set; }
    public string? DES_RESULTADO { get; set; }
}

public class DashboardBdIoLatencia
{
    public string?  DES_TIPO         { get; set; }  // DATOS | LOG
    public decimal? VAL_MS_LECTURA   { get; set; }
    public decimal? VAL_MS_ESCRITURA { get; set; }
    public decimal? VAL_MB_LEIDOS    { get; set; }
    public decimal? VAL_MB_ESCRITOS  { get; set; }
    public long     NUM_LECTURAS     { get; set; }
    public long     NUM_ESCRITURAS   { get; set; }
}

public class DashboardBdProcedimiento
{
    public string?   DES_PROCEDIMIENTO    { get; set; }
    public long      NUM_EJECUCIONES      { get; set; }
    public decimal   VAL_DURACION_PROM_MS { get; set; }
    public decimal   VAL_CPU_PROM_MS      { get; set; }
    public long      VAL_LECTURAS_PROM    { get; set; }
    public DateTime? FEC_ULTIMA_EJECUCION { get; set; }
}

public class SpTraceEntry
{
    public DateTime FEC_HORA        { get; set; }
    public string   DES_SP          { get; set; } = string.Empty;
    public string   DES_EXEC        { get; set; } = string.Empty;
    public long     VAL_DURACION_MS { get; set; }
}
