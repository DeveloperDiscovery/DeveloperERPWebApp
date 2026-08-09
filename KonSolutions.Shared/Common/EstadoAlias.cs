namespace KONSolutions.Shared.Common;

/// <summary>Alias de estado estándar (ESTANDAR.ESTADOS.COD_ESTADO_ALIAS) y la regla de
/// negocio transversal asociada.
///
/// REGLA GENERAL DE LA APLICACIÓN: un registro cuyo estado tenga alias 'ANU' (ANULADO)
/// queda CONGELADO — solo se puede consultar. No admite edición, eliminación ni ninguna
/// acción que lo modifique (asignar responsable, reasignar asesor, agregar memorias, etc.).
/// Aplica a todos los módulos; ver uso en Comercial/SolicitudesComerciales*.razor e
/// Ingenieria/Disenos*.razor.</summary>
public static class EstadoAlias
{
    public const string Anulado = "ANU";
    public const string Activo = "ACT";
    public const string Asignado = "ASG";

    /// <summary>True si el alias corresponde a un registro anulado (solo lectura).
    /// Tolera null/espacios y diferencias de mayúsculas: si el SP todavía no devuelve
    /// COD_ESTADO_ALIAS, el registro NO se bloquea (degradación segura).</summary>
    public static bool EsAnulado(string? codEstadoAlias) =>
        string.Equals(codEstadoAlias?.Trim(), Anulado, StringComparison.OrdinalIgnoreCase);
}
