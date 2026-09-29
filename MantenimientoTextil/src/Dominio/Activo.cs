namespace MantenimientoTextil.Dominio;

/// <summary>RF-MNT-001. CodigoMaestro debe coincidir con el maestro de artículos del ERP.</summary>
public class Activo
{
    public int ActivoId { get; init; }
    public required string CodigoMaestro { get; init; }
    public required string Nombre { get; init; }
    public int? ActivoPadreId { get; init; }
    public Criticidad Criticidad { get; init; } = Criticidad.C;
}
