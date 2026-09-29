using MantenimientoTextil.Dominio;
using Xunit;

namespace Dominio.Tests;

public class PlanMantenimientoTests
{
    [Fact] // RF-MNT-011
    public void ProximoVencimientoPorTiempo_SumaIntervaloEnDias()
    {
        var plan = new PlanMantenimiento { Nombre = "Lubricación", Tipo = TipoPlan.Tiempo, Intervalo = 30 };
        Assert.Equal(new DateTime(2026, 2, 1), plan.ProximoVencimientoPorTiempo(new DateTime(2026, 1, 2)));
    }

    [Fact] // RF-MNT-011
    public void EstaVencidoPorContador_EnElLimiteExacto_EsVerdadero()
    {
        var plan = new PlanMantenimiento { Nombre = "Cambio de aceite", Tipo = TipoPlan.Contador, Intervalo = 500 };
        Assert.True(plan.EstaVencidoPorContador(1000, 1500));
        Assert.False(plan.EstaVencidoPorContador(1000, 1499));
    }

    [Fact] // RF-MNT-011
    public void ProximoVencimientoPorTiempo_ConPlanPorContador_Falla()
    {
        var plan = new PlanMantenimiento { Nombre = "X", Tipo = TipoPlan.Contador, Intervalo = 10 };
        Assert.Throws<InvalidOperationException>(() => plan.ProximoVencimientoPorTiempo(DateTime.Today));
    }
}
