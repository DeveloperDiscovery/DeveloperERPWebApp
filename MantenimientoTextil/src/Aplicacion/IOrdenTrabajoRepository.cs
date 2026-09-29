using MantenimientoTextil.Dominio;

namespace MantenimientoTextil.Aplicacion;

/// <summary>Puerto de persistencia (RNF-001): la implementación Access puede cambiarse por SQL Server.</summary>
public interface IOrdenTrabajoRepository
{
    Task<OrdenTrabajo?> ObtenerAsync(int ordenId, CancellationToken ct = default);
}
