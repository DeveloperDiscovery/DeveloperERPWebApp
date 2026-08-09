// filtroExcelColumna.js — cierra el popup de FiltroExcelColumna.razor cuando el tab
// del Workspace donde vive la grilla deja de estar activo (WorkspaceTabHost solo
// alterna display:none/flex en ".ws-tab-pane", no destruye el contenido — así que el
// popup, al vivir en un portal fuera de ese árbol, quedaba abierto "flotando" al
// cambiar de pestaña sin que el usuario lo cerrara).
window.filtroExcelColumna = {
    _observers: new WeakMap(),

    observarTab(root, dotNetRef) {
        const pane = root && root.closest('.ws-tab-pane');
        if (!pane) return;
        const obs = new MutationObserver(() => {
            if (pane.style.display === 'none') {
                dotNetRef.invokeMethodAsync('CerrarDesdeJs');
            }
        });
        obs.observe(pane, { attributes: true, attributeFilter: ['style'] });
        this._observers.set(root, obs);
    },

    dispose(root) {
        const obs = this._observers.get(root);
        if (obs) { obs.disconnect(); this._observers.delete(root); }
    },

    // Lee la fuente/tamaño REALMENTE renderizados en la primera fila de la grilla
    // contenedora (computed style), en vez de confiar en qué variable CSS termina
    // ganando la cascada — así el filtro queda visualmente idéntico a la grilla
    // sin importar de dónde salga el valor final.
    //
    // OJO: el popup de MudMenu se porta fuera del contenedor con transform:scale que
    // aplica el zoom propio de la app (appZoom.js, sobre #app) — por eso, aunque la
    // grilla y el popup reporten el MISMO font-size "declarado" (ej. 14px), en pantalla
    // se ven distintos: la grilla queda físicamente escalada por ese transform y el
    // popup no. Hay que multiplicar el tamaño leído por el factor de zoom actual para
    // que el resultado en pantalla coincida de verdad.
    obtenerFuenteGrilla(root) {
        if (!root) return null;
        const contenedor = root.closest(
            '.mud-table-container, .mud-data-grid, .mud-table, [data-resizable-grid]'
        ) || document;
        // Si el filtro dejó la grilla en 0 filas, no hay celda de dato que leer — se cae
        // al header (siempre presente) para no perder el tamaño ya calculado.
        const celda = contenedor.querySelector(
            'tbody .mud-table-cell, tbody td, .mud-table-body .mud-table-cell, .mud-table-body td, .mud-table-row td'
        ) || contenedor.querySelector(
            'thead .mud-table-cell, thead th, .mud-table-head .mud-table-cell, .mud-table-head th'
        );
        if (!celda) return null;
        const estilo = getComputedStyle(celda);
        const escala = (window.appZoom && typeof window.appZoom.getScale === 'function')
            ? (window.appZoom.getScale() || 1) : 1;
        const tamanoPx = parseFloat(estilo.fontSize) || 14;
        return { fontFamily: estilo.fontFamily, fontSize: (tamanoPx * escala) + 'px' };
    }
};
