// workspaceTabHost.js — al cambiar de pestaña del Workspace, cierra cualquier popover/menú
// que haya quedado abierto (ej. FiltroExcelColumna, AriaCombo, o cualquier MudMenu/MudPopover).
// Esos popovers viven en un portal fuera del árbol de la pestaña, que WorkspaceTabHost solo
// oculta con display:none (no lo destruye) — así que cambiar de pestaña no los cerraba solo,
// y quedaban "flotando" con la posición vieja.
//
// Un click sintético en document (mousedown/click) NO sirve acá: MudBlazor no escucha clicks
// a nivel de document para cerrar sus popovers, así que se ocultan los popovers visibles
// directamente por CSS (display:none) — más contundente, no depende de que MudBlazor
// reaccione a un evento simulado.
window.workspaceTabHost = {
    cerrarPopoversAbiertos() {
        // OJO: estos popovers usan position:fixed — offsetParent SIEMPRE es null en ese
        // caso (sea visible o no), así que la visibilidad real hay que chequearla con el
        // estilo computado (display/visibility), no con offsetParent.
        document.querySelectorAll('.mud-popover').forEach(function (pop) {
            if (pop.childElementCount === 0) return; // nunca se abrió, MudBlazor lo deja vacío
            const estilo = getComputedStyle(pop);
            if (estilo.display !== 'none' && estilo.visibility !== 'hidden') {
                pop.style.display = 'none';
            }
        });
        document.querySelectorAll('.mud-overlay').forEach(function (ov) {
            const estilo = getComputedStyle(ov);
            if (estilo.display !== 'none' && estilo.visibility !== 'hidden') {
                ov.style.display = 'none';
            }
        });
    }
};
