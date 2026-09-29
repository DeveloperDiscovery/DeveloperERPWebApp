namespace MantenimientoTextil.Dominio;

/// <summary>RF-MNT-010 / RF-MNT-011. Intervalo en días (Tiempo) o unidades del contador (Contador).</summary>
public class PlanMantenimiento
{
    public int PlanId { get; init; }
    public int ActivoId { get; init; }
    public required string Nombre { get; init; }
    public TipoPlan Tipo { get; init; }
    public double Intervalo { get; init; }
    public double DuracionEstimadaHoras { get; init; }

    public DateTime ProximoVencimientoPorTiempo(DateTime ultimaEjecucion)
    {
        if (Tipo != TipoPlan.Tiempo) throw new InvalidOperationException("El plan no es por tiempo.");
        return ultimaEjecucion.AddDays(Intervalo);
    }

    public double ProximoVencimientoPorContador(double lecturaUltimaEjecucion)
    {
        if (Tipo != TipoPlan.Contador) throw new InvalidOperationException("El plan no es por contador.");
        return lecturaUltimaEjecucion + Intervalo;
    }

    public bool EstaVencidoPorContador(double lecturaUltimaEjecucion, double lecturaActual) =>
        lecturaActual >= ProximoVencimientoPorContador(lecturaUltimaEjecucion);
}
