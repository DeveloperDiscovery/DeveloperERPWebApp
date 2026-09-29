namespace MantenimientoTextil.Dominio;

/// <summary>RF-MNT-030. Ciclo: Solicitada → Planificada → Programada → EnEjecucion → Cerrada.</summary>
public class OrdenTrabajo
{
    public int OrdenId { get; init; }
    public int ActivoId { get; init; }
    public TipoOrden Tipo { get; init; }
    public EstadoOrden Estado { get; private set; } = EstadoOrden.Solicitada;

    public void Avanzar()
    {
        if (Estado == EstadoOrden.Cerrada)
            throw new InvalidOperationException("La orden ya está cerrada.");
        Estado = (EstadoOrden)((int)Estado + 1);
    }
}
