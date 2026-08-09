namespace KONSolutions.Web.Services;

/// <summary>
/// Portapapeles de color compartido entre todas las instancias de ColorConSwatch
/// en toda la app (registrado como Scoped en Program.cs). Copiar un color en
/// cualquier input y notificar a los demás para que su botón "Pegar" se habilite
/// de inmediato, sin esperar a que se re-rendericen por otra razón.
/// </summary>
public class ColorClipboardService
{
    public string? Color { get; private set; }

    public event Action? OnChange;

    public void Copiar(string? color)
    {
        Color = color;
        OnChange?.Invoke();
    }
}
