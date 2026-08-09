namespace KONSolutions.Web.Services;

/// <summary>
/// Bus de atajos de teclado globales (Ctrl+N / Ctrl+S / Delete / F2).
/// MainLayout escucha el keydown global y dispara estos eventos; las páginas
/// CRUD que quieran responder a los atajos solo necesitan suscribirse en
/// OnInitialized y desuscribirse en Dispose.
/// </summary>
public class ShortcutService
{
    public event Action? NuevoRequested;
    public event Action? GuardarRequested;
    public event Action? EliminarRequested;
    public event Action? ConsultarRequested;

    public void RaiseNuevo()     => NuevoRequested?.Invoke();
    public void RaiseGuardar()   => GuardarRequested?.Invoke();
    public void RaiseEliminar()  => EliminarRequested?.Invoke();
    public void RaiseConsultar() => ConsultarRequested?.Invoke();
}
