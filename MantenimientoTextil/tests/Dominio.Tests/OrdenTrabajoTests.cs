using MantenimientoTextil.Dominio;
using Xunit;

namespace Dominio.Tests;

public class OrdenTrabajoTests
{
    [Fact] // RF-MNT-030
    public void Avanzar_RecorreElCicloCompleto()
    {
        var ot = new OrdenTrabajo { Tipo = TipoOrden.Correctivo };
        for (var i = 0; i < 4; i++) ot.Avanzar();
        Assert.Equal(EstadoOrden.Cerrada, ot.Estado);
    }

    [Fact] // RF-MNT-030
    public void Avanzar_DesdeCerrada_Falla()
    {
        var ot = new OrdenTrabajo { Tipo = TipoOrden.Preventivo };
        for (var i = 0; i < 4; i++) ot.Avanzar();
        Assert.Throws<InvalidOperationException>(() => ot.Avanzar());
    }
}
