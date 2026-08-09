namespace KONSolutions.Web.Services;

/// <summary>
/// Caché en memoria (por sesión) del set de favoritos del usuario, para que las
/// pantallas de menú (circular, tiles) no tengan que volver a pedirlos a la API
/// cada una por su cuenta — MainLayout ya los carga una vez al login (en paralelo,
/// una ronda por todas las apps) y los deja aquí; el resto solo lee en memoria.
/// </summary>
public class FavoritosCacheService
{
    public HashSet<string> Set { get; } = new();
    public bool Cargado { get; private set; }

    public bool EsFavorito(string codApp, string codOpcion) => Set.Contains($"{codApp}|{codOpcion}");

    public void Establecer(IEnumerable<(string codApp, string codOpcion)> items)
    {
        Set.Clear();
        foreach (var (codApp, codOpcion) in items)
            Set.Add($"{codApp}|{codOpcion}");
        Cargado = true;
    }

    public void Agregar(string codApp, string codOpcion) => Set.Add($"{codApp}|{codOpcion}");
    public void Quitar(string codApp, string codOpcion) => Set.Remove($"{codApp}|{codOpcion}");
}
