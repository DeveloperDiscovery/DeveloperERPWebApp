namespace KONSolutions.Web.Services;

/// <summary>
/// Helper para navegación de grillas con teclas de dirección (↑/↓) y Enter.
/// Cada página mantiene su propio _filaFoco (el item actualmente resaltado) y usa
/// estos métodos genéricos para moverlo arriba/abajo dentro de la lista visible.
///
/// Patrón de uso en una página con MudDataGrid:
///   1. Inyectar IJSRuntime.
///   2. Mantener un campo `_filaFoco` del tipo T (nullable).
///   3. RowClick="@(e => _filaFoco = e.Item)"
///   4. RowClassFunc="@((item, index) => GridKeyNavHelper.RowClass(item, index, _filaFoco))"
///   5. Envolver el MudDataGrid en un &lt;div id="@_gridId" class="gridkeynav-container"
///      @onkeydown="@(e => OnKeyDown(e))"&gt;, con OnKeyDown:
///        if (!GridKeyNavHelper.EsTeclaNavegacion(e.Key)) return;
///        _filaFoco = GridKeyNavHelper.CalcularSiguienteFoco(e.Key, _items, _filaFoco);
///        StateHasChanged();
///   6. En OnAfterRenderAsync(firstRender): await JS.InvokeVoidAsync("gridKeyNav.attach", _gridId);
/// </summary>
public static class GridKeyNavHelper
{
    /// <summary>Clase CSS para la fila actualmente enfocada — usar en RowClassFunc del MudDataGrid.
    /// RowClassFunc en MudBlazor 7.15 recibe (item, index) → string.</summary>
    public static string RowClass<T>(T item, int index, T? filaFoco) where T : class
        => EqualityComparer<T>.Default.Equals(item, filaFoco) ? "gridkeynav-row gridkeynav-focused" : "gridkeynav-row";

    /// <summary>Calcula el siguiente item de foco según la tecla presionada, sin usar ref
    /// (los parámetros ref/out no son válidos dentro de lambdas en C#).
    /// Retorna null si la tecla no es de navegación o no hay cambio.</summary>
    public static T? CalcularSiguienteFoco<T>(string key, IReadOnlyList<T> items, T? filaFocoActual) where T : class
    {
        if (items.Count == 0) return filaFocoActual;

        var idx = filaFocoActual is null ? -1 : items.ToList().FindIndex(i => EqualityComparer<T>.Default.Equals(i, filaFocoActual));

        switch (key)
        {
            case "ArrowDown":
                idx = idx < 0 ? 0 : Math.Min(idx + 1, items.Count - 1);
                return items[idx];
            case "ArrowUp":
                idx = idx < 0 ? 0 : Math.Max(idx - 1, 0);
                return items[idx];
            default:
                return filaFocoActual;
        }
    }

    /// <summary>True si la tecla es una tecla de navegación manejada (ArrowUp/ArrowDown).</summary>
    public static bool EsTeclaNavegacion(string key) => key is "ArrowUp" or "ArrowDown";

    /// <summary>
    /// Throttle simple: evita procesar más de un evento de teclado cada N milisegundos.
    /// Cada página mantiene su propio campo `DateTime _ultimoKeyDown`. Llamar al inicio
    /// del OnKeyDown: si retorna null, ignorar el evento; si no, actualizar el campo
    /// con el valor devuelto y continuar procesando.
    /// Evita que el "key repeat" del navegador sature los renders de Blazor,
    /// que es la causa típica de que la navegación se sienta lenta o se trabe.
    /// </summary>
    public static DateTime? PuedeProcesar(DateTime ultimoKeyDown, int minMs = 60)
    {
        var ahora = DateTime.UtcNow;
        return (ahora - ultimoKeyDown).TotalMilliseconds < minMs ? null : ahora;
    }
}
