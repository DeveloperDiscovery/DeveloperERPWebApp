// workspaceDialogScope.js — hace que un MudDialog abierto desde una pestaña del Workspace
// quede "atado" a esa pestaña, en vez de ser un overlay global que bloquea TODA la app
// sin importar a cuál pestaña cambies.
//
// MudDialogProvider se monta una sola vez fuera del árbol de pestañas (WorkspaceTabHost
// solo hace display:none/flex en ".ws-tab-pane", nunca destruye nada) — por eso un diálogo
// abierto en la pestaña A queda flotando encima de CUALQUIER otra pestaña a la que cambies,
// "congelando" toda la web en vez de solo esa pestaña.
//
// Solución: cuando aparece un nuevo overlay/diálogo, se etiqueta con el id de la pestaña que
// estaba activa en ese momento (data-tab-id). Al cambiar de pestaña (ActivarTab en
// WorkspaceTabHost.razor), se ocultan los que no pertenecen a la pestaña recién activada, y
// se muestran los que sí.
//
// BUG encontrado (dos pestañas con diálogo abierto cada una): Blazor puede REUTILIZAR el
// mismo contenedor DOM (.mud-overlay/.mud-dialog-container) para el SIGUIENTE diálogo que
// se abre, en vez de insertar un nodo nuevo — el MutationObserver solo reacciona a la
// PRIMERA inserción, así que el segundo diálogo heredaba el data-tab-id viejo (el de la
// primera pestaña) en vez del de la pestaña donde realmente se abrió. Por eso se re-etiqueta
// el contenedor en CADA mutación relevante (no solo cuando el contenedor mismo se inserta),
// usando la pestaña activa guardada en _tabActivo (actualizada en cada cambio de pestaña),
// no una lectura puntual del DOM al momento de la inserción.
window.workspaceDialogScope = {
    _observer: null,
    _tabActivo: null,

    _esRelevante(nodo) {
        if (!nodo || nodo.nodeType !== 1) return false;
        if (nodo.matches && nodo.matches('.mud-overlay, .mud-dialog-container, .mud-dialog, [role="dialog"]')) return true;
        if (nodo.querySelector && nodo.querySelector('.mud-dialog, [role="dialog"]')) return true;
        return false;
    },

    // Contenedor externo (overlay o dialog-container) más cercano al nodo mutado, sobre
    // el que actualizarVisibilidad() decide mostrar/ocultar. Si no hay un ancestro así,
    // se usa el propio nodo (fallback si esta versión de MudBlazor no anida como se espera).
    _contenedorDe(nodo) {
        let n = nodo;
        while (n && n !== document.body) {
            if (n.nodeType === 1 && n.matches && n.matches('.mud-overlay, .mud-dialog-container')) return n;
            n = n.parentElement;
        }
        return nodo;
    },

    iniciar(tabIdActivo) {
        if (tabIdActivo) this._tabActivo = tabIdActivo;
        if (this._observer) return; // ya está observando, no duplicar
        this._observer = new MutationObserver((mutaciones) => {
            for (const m of mutaciones) {
                m.addedNodes.forEach((nodo) => {
                    if (!this._esRelevante(nodo)) return;
                    if (!this._tabActivo) return;
                    const contenedor = this._contenedorDe(nodo);
                    contenedor.dataset.tabId = this._tabActivo;
                });
            }
        });
        this._observer.observe(document.body, { childList: true, subtree: true });
    },

    actualizarVisibilidad(tabIdActivo) {
        this._tabActivo = tabIdActivo;
        document.querySelectorAll('[data-tab-id]').forEach((el) => {
            if (!el.matches('.mud-overlay, .mud-dialog-container, .mud-dialog, [role="dialog"]')) return;
            el.style.display = (el.dataset.tabId === tabIdActivo) ? '' : 'none';
        });
    }
};
