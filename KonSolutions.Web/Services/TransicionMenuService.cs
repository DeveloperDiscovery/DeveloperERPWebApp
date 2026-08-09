namespace KONSolutions.Web.Services;

/// <summary>
/// Bus simple para mostrar/ocultar el overlay de "cargando" del layout (el mismo estilo
/// de la pantalla posterior al login) durante transiciones cortas, como el giro del menú
/// circular: se abre y activa la pestaña destino de inmediato (por debajo, ya con layout
/// real — nada de display:none), y el overlay tapa la transición hasta que se decide
/// ocultarlo, evitando así cualquier parpadeo de contenido a medio cargar.
/// </summary>
public class TransicionMenuService
{
    public event Action<string>? OnMostrar;
    public event Action? OnOcultar;

    public void Mostrar(string mensaje = "Abriendo...") => OnMostrar?.Invoke(mensaje);
    public void Ocultar() => OnOcultar?.Invoke();
}
