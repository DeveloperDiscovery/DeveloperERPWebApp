using System.Data.OleDb;
using MantenimientoTextil.Aplicacion;
using MantenimientoTextil.Dominio;

namespace MantenimientoTextil.Infraestructura.Access;

public class OrdenTrabajoRepository(ConexionAccess conexion) : IOrdenTrabajoRepository
{
    public async Task<OrdenTrabajo?> ObtenerAsync(int ordenId, CancellationToken ct = default)
    {
        await using var cn = conexion.Crear();
        await cn.OpenAsync(ct);
        await using var cmd = new OleDbCommand("SELECT OrdenId, ActivoId, Tipo FROM OrdenTrabajo WHERE OrdenId = ?", cn);
        cmd.Parameters.AddWithValue("@OrdenId", ordenId);
        await using var rd = await cmd.ExecuteReaderAsync(ct);
        if (!await rd.ReadAsync(ct)) return null;
        return new OrdenTrabajo
        {
            OrdenId = rd.GetInt32(0),
            ActivoId = rd.GetInt32(1),
            Tipo = Enum.Parse<TipoOrden>(rd.GetString(2))
        };
    }
}
