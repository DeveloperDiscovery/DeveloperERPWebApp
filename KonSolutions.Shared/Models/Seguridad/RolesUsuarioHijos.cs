using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Seguridad;

// ═══════════════════════════════════════════════════════════════════════════════
// Hijos de SEGURIDAD.ROLES_USUARIO.
//
// Los cuatro son tablas de asignación pura: fuera de la auditoría no tienen datos propios.
// Por eso ninguno tiene diálogo de alta/edición — la pantalla es siempre el mismo par de
// grillas «asignados | disponibles» con switch de marcado y menú contextual.
//
// Los "disponibles" los resuelve un SP _DISPONIBLES en la base y no una resta en el cliente:
// la base ya sabe cuáles están anulados y cuáles corresponden al almacén.
// ═══════════════════════════════════════════════════════════════════════════════

// ───────────── ROLES_USUARIO_ALMACENES ─────────────
/// <summary>Almacenes que un rol puede operar. PK: COD_ROL_USUARIO + COD_ALMACEN.
/// El mismo tipo sirve para los asignados y para los disponibles: el SP _DISPONIBLES trae
/// COD_ALMACEN y DES_ALMACEN, que son justo las dos columnas de la grilla.</summary>
public class RolAlmacen
{
    [Required] public string COD_ROL_USUARIO { get; set; } = "";
    [Required] public string COD_ALMACEN     { get; set; } = "";

    public string    COD_USUARIO_REGISTRO  { get; set; } = "";
    public string    COD_ESTACION_REGISTRO { get; set; } = "";
    public DateTime? FEC_REGISTRO          { get; set; }

    // Solo lectura (JOIN).
    public string? DES_ROL_USUARIO { get; set; }
    public string? DES_ALMACEN     { get; set; }
    public string? DES_ABREVIADA   { get; set; }
    public string? DES_POLIGONO    { get; set; }
}

/// <summary>Lote de almacenes en una sola llamada. Quince marcados no deberían ser quince
/// peticiones: si una falla por el medio, el rol queda a medio configurar y sin forma de
/// saber cuáles entraron.</summary>
public class RolAlmacenesLote
{
    [Required] public string COD_ROL_USUARIO { get; set; } = "";
    public List<string> Almacenes { get; set; } = new();
}

// ───────────── ROLES_USUARIO_ALMACENES_TIPOS_MOVIMIENTO ─────────────
/// <summary>Tipos de movimiento que un rol puede hacer DENTRO de un almacén ya asignado.
/// Hijo de RolAlmacen. PK: rol + almacén + clase + tipo.
///
/// El universo de disponibles sale de LOGISTICA.ALMACENES_TIPOS_MOVIMIENTO: un rol no puede
/// recibir permiso sobre un movimiento que el almacén no admite.</summary>
public class RolAlmacenTipoMovimiento
{
    [Required] public string COD_ROL_USUARIO      { get; set; } = "";
    [Required] public string COD_ALMACEN          { get; set; } = "";
    [Required] public string COD_CLASE_MOVIMIENTO { get; set; } = "";
    [Required] public string COD_TIPO_MOVIMIENTO  { get; set; } = "";

    public string    COD_USUARIO_REGISTRO  { get; set; } = "";
    public string    COD_ESTACION_REGISTRO { get; set; } = "";
    public DateTime? FEC_REGISTRO          { get; set; }

    // Solo lectura (JOIN).
    public string? DES_ROL_USUARIO      { get; set; }
    public string? DES_ALMACEN          { get; set; }
    public string? DES_ABREVIADA        { get; set; }
    public string? DES_POLIGONO         { get; set; }
    public string? DES_CLASE_MOVIMIENTO { get; set; }
    public string? DES_TIPO_MOVIMIENTO  { get; set; }
    public string? DES_OBSERVACION      { get; set; }
}

/// <summary>Par clase+tipo: el código de tipo sólo identifica un movimiento dentro de su
/// clase, así que el lote no puede ser una lista de códigos sueltos.</summary>
public class ClaseTipoMovimiento
{
    public string COD_CLASE_MOVIMIENTO { get; set; } = "";
    public string COD_TIPO_MOVIMIENTO  { get; set; } = "";
}

public class RolAlmacenTiposMovimientoLote
{
    [Required] public string COD_ROL_USUARIO { get; set; } = "";
    [Required] public string COD_ALMACEN     { get; set; } = "";
    public List<ClaseTipoMovimiento> Tipos { get; set; } = new();
}

// ───────────── ROLES_USUARIO_TIPOS_OPERACION ─────────────
/// <summary>Tipos de operación habilitados para un rol. PK: rol + clase + tipo.
///
/// Asimetría de los SP: los asignados se leen por rol Y clase, pero los disponibles vienen de
/// todas las clases juntas. Por eso la pantalla filtra los disponibles en memoria.</summary>
public class RolTipoOperacion
{
    [Required] public string COD_ROL_USUARIO     { get; set; } = "";
    [Required] public string COD_CLASE_OPERACION { get; set; } = "";
    [Required] public string COD_TIPO_OPERACION  { get; set; } = "";

    public string    COD_USUARIO_REGISTRO  { get; set; } = "";
    public string    COD_ESTACION_REGISTRO { get; set; } = "";
    public DateTime? FEC_REGISTRO          { get; set; }

    // Solo lectura (JOIN).
    public string? DES_ROL_USUARIO     { get; set; }
    public string? DES_CLASE_OPERACION { get; set; }
    public string? DES_TIPO_OPERACION  { get; set; }
}

public class ClaseTipoOperacion
{
    public string COD_CLASE_OPERACION { get; set; } = "";
    public string COD_TIPO_OPERACION  { get; set; } = "";
}

public class RolTiposOperacionLote
{
    [Required] public string COD_ROL_USUARIO { get; set; } = "";
    public List<ClaseTipoOperacion> Tipos { get; set; } = new();
}

// ───────────── ROLES_USUARIO_ASIENTOS_CONTABLES ─────────────
/// <summary>Asientos contables habilitados para un rol. PK: rol + asiento.
/// El SP _DISPONIBLES ya excluye los asientos anulados.</summary>
public class RolAsientoContable
{
    [Required] public string COD_ROL_USUARIO      { get; set; } = "";
    [Required] public string COD_ASIENTO_CONTABLE { get; set; } = "";

    public string    COD_USUARIO_REGISTRO  { get; set; } = "";
    public string    COD_ESTACION_REGISTRO { get; set; } = "";
    public DateTime? FEC_REGISTRO          { get; set; }

    // Solo lectura (JOIN).
    public string? DES_ROL_USUARIO      { get; set; }
    public string? DES_ASIENTO_CONTABLE { get; set; }
}

public class RolAsientosContablesLote
{
    [Required] public string COD_ROL_USUARIO { get; set; } = "";
    public List<string> Asientos { get; set; } = new();
}

// ───────────── Resultado común de los lotes ─────────────
/// <summary>Qué pasó con el lote. Los fallidos se informan uno por uno: es normal que alguna
/// baja sea rechazada porque el registro ya está en uso, y el usuario necesita saber cuál.</summary>
public class RolAsignacionLoteResult
{
    public int Procesados { get; set; }
    public List<string> Fallidos { get; set; } = new();
    public string? DesError { get; set; }
}
