using Microsoft.AspNetCore.Components;

namespace KONSolutions.Web.Services;

/// <summary>Una pestaña abierta en el workspace.</summary>
public class WorkspaceTab
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Titulo { get; set; } = "";
    public string Icono { get; set; } = "";
    /// <summary>Ícono propio de la opción en la BD (base64), si existe. Tiene prioridad
    /// sobre <see cref="Icono"/> (el ícono fijo del menú tile/circular/lateral), que
    /// sirve de reserva cuando la opción no tiene un ícono propio configurado.</summary>
    public string? IconoImagenBase64 { get; set; }
    public Type ComponentType { get; set; } = default!;
    public Dictionary<string, object> Parametros { get; set; } = new();
    public string? Ruta { get; set; }
    public bool EsCerrable { get; set; } = true;
    public int Panel { get; set; } = 0;   // 0 = panel principal  1 = panel secundario
    /// <summary>True mientras la pestaña se abrió con activar:false (precarga en segundo
    /// plano, ej. durante el giro del menú circular): su componente ya está montado y
    /// cargando, pero todavía no debe aparecer en la barra de pestañas ni como activa.</summary>
    public bool Precargando { get; set; }
}

/// <summary>
/// Gestiona las pestañas internas del workspace con soporte de panel dividido.
/// Panel 0 = siempre visible. Panel 1 = aparece al mover la primera pestaña ahí.
/// </summary>
public class WorkspaceTabService
{
    public List<WorkspaceTab> Tabs { get; } = new();

    // Tab activa por panel
    public string? ActiveTabId  { get; private set; }   // panel 0
    public string? ActiveTabIdB { get; private set; }   // panel 1

    // Dirección del split: "h" = horizontal (arriba/abajo), "v" = vertical (izq/der)
    public string SplitDireccion { get; private set; } = "h";

    public bool TieneSplit => Tabs.Any(t => t.Panel == 1);

    public event Action? OnChange;

    // ── helpers ──────────────────────────────────────────────────────────────

    public IEnumerable<WorkspaceTab> TabsDePanel(int panel) => Tabs.Where(t => t.Panel == panel);

    public string? GetActiveTabId(int panel) => panel == 0 ? ActiveTabId : ActiveTabIdB;

    private void SetActiveTabId(int panel, string? id)
    {
        if (panel == 0) ActiveTabId  = id;
        else            ActiveTabIdB = id;
    }

    // ── abrir / activar ──────────────────────────────────────────────────────

    /// <summary>
    /// Abre (o activa, si ya existe por Ruta) una pestaña.
    /// <paramref name="activar"/>=false permite crear la pestaña y dejar que su componente
    /// empiece a cargar en segundo plano (WorkspaceTabHost monta todas las pestañas abiertas,
    /// ocultas con display:none) SIN cambiar la pestaña visible todavía — útil para
    /// precargar el destino mientras se reproduce una animación (ej. giro del menú circular)
    /// y activarlo recién al terminar, vía <see cref="ActivarPorRuta"/>.
    /// </summary>
    public void Abrir(string titulo, string icono, Type componentType, string? ruta = null,
                      Dictionary<string, object>? parametros = null, bool esCerrable = true,
                      bool alInicio = false, string? iconoImagenBase64 = null, bool activar = true)
    {
        if (!string.IsNullOrEmpty(ruta))
        {
            var existente = Tabs.FirstOrDefault(t => t.Ruta == ruta);
            if (existente is not null)
            {
                if (activar) SetActiveTabId(existente.Panel, existente.Id);
                NotifyChanged();
                return;
            }
        }

        var tab = new WorkspaceTab
        {
            Titulo = titulo, Icono = icono, IconoImagenBase64 = iconoImagenBase64, ComponentType = componentType,
            Ruta = ruta, Parametros = parametros ?? new(), EsCerrable = esCerrable,
            Panel = 0, Precargando = !activar
        };
        if (alInicio) Tabs.Insert(0, tab);
        else          Tabs.Add(tab);
        if (activar) ActiveTabId = tab.Id;
        NotifyChanged();
    }

    /// <summary>Activa (hace visible) una pestaña ya abierta, identificada por su Ruta.
    /// Usado tras precargar una pestaña con Abrir(..., activar:false).</summary>
    public void ActivarPorRuta(string? ruta)
    {
        if (string.IsNullOrEmpty(ruta)) return;
        var tab = Tabs.FirstOrDefault(t => t.Ruta == ruta);
        if (tab is null) return;
        tab.Precargando = false;
        SetActiveTabId(tab.Panel, tab.Id);
        NotifyChanged();
    }

    public void AbrirWorkspace(string titulo, string icono, Type componentType, string ruta)
    {
        if (Tabs.Any(t => t.Ruta == ruta))
        {
            var ws = Tabs.First(t => t.Ruta == ruta);
            SetActiveTabId(ws.Panel, ws.Id);
            NotifyChanged();
            return;
        }
        var tab = new WorkspaceTab
        {
            Titulo = titulo, Icono = icono, ComponentType = componentType,
            Ruta = ruta, EsCerrable = false, Panel = 0
        };
        Tabs.Insert(0, tab);
        ActiveTabId = tab.Id;
        NotifyChanged();
    }

    public void Activar(string tabId)
    {
        var tab = Tabs.FirstOrDefault(t => t.Id == tabId);
        if (tab is null) return;
        SetActiveTabId(tab.Panel, tabId);
        NotifyChanged();
    }

    // ── cerrar ───────────────────────────────────────────────────────────────

    public void Cerrar(string tabId)
    {
        var idx = Tabs.FindIndex(t => t.Id == tabId);
        if (idx < 0) return;
        var tab = Tabs[idx];
        if (!tab.EsCerrable) return;

        var panel = tab.Panel;
        Tabs.RemoveAt(idx);

        var activo = GetActiveTabId(panel);
        if (activo == tabId)
        {
            var vecinos = Tabs.Where(t => t.Panel == panel).ToList();
            SetActiveTabId(panel, vecinos.Count == 0 ? null : vecinos[Math.Min(idx, vecinos.Count) - 1 < 0 ? 0 : Math.Min(idx - 1, vecinos.Count - 1)].Id);
        }
        NotifyChanged();
    }

    public void CerrarTodas()
    {
        Tabs.Clear();
        ActiveTabId = ActiveTabIdB = null;
        NotifyChanged();
    }

    public void CerrarTodasMenosFijas()
    {
        Tabs.RemoveAll(t => t.EsCerrable);
        ActiveTabId  = Tabs.FirstOrDefault(t => t.Panel == 0)?.Id;
        ActiveTabIdB = Tabs.FirstOrDefault(t => t.Panel == 1)?.Id;
        NotifyChanged();
    }

    // ── split ─────────────────────────────────────────────────────────────────

    /// <summary>Mueve un tab al panel indicado (0 o 1).</summary>
    public void MoverAPanel(string tabId, int panelDestino)
    {
        var tab = Tabs.FirstOrDefault(t => t.Id == tabId);
        if (tab is null || tab.Panel == panelDestino) return;

        var panelOrigen = tab.Panel;
        tab.Panel = panelDestino;

        // Activar en el destino
        SetActiveTabId(panelDestino, tabId);

        // En el origen, activar otro tab si este era el activo
        if (GetActiveTabId(panelOrigen) == tabId || GetActiveTabId(panelOrigen) == null)
        {
            var vecino = Tabs.FirstOrDefault(t => t.Panel == panelOrigen);
            SetActiveTabId(panelOrigen, vecino?.Id);
        }

        NotifyChanged();
    }

    public void CambiarDireccionSplit()
    {
        SplitDireccion = SplitDireccion == "h" ? "v" : "h";
        NotifyChanged();
    }

    private void NotifyChanged() => OnChange?.Invoke();
}
