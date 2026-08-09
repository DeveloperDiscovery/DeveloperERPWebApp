// AriaCombo — soporte JS mínimo para el patrón W3C ARIA Combobox (AriaCombo.razor).
// El teclado (flechas/Enter/Esc) se maneja en C# vía @onkeydown; este módulo solo cubre
// lo que Blazor no puede hacer bien por sí solo: cerrar al clickear afuera (sin depender
// del focus-trap de MudBlazor, que fue lo que generaba el warning de aria-hidden) y
// desplazar la opción activa a la vista dentro del listbox.
window.ariaCombo = {
    _handlers: new WeakMap(),

    init(root, dotNetRef) {
        const handler = (ev) => {
            if (root && !root.contains(ev.target)) {
                // Un mousedown fuera de CUALQUIER combo abierto dispara este handler en
                // TODOS los AriaCombo vivos del documento a la vez (ej. al cerrar el
                // diálogo con un clic en "Cancelar"/"Modificar"). Si ese mismo clic
                // también dispara la destrucción del componente, Blazor puede desregistrar
                // el DotNetObjectReference ANTES de que esta llamada asíncrona llegue a
                // ejecutarse — invokeMethodAsync entonces rechaza con "There is no tracked
                // object with id X... already disposed". Es una carrera inofensiva (el
                // combo ya no existe, así que "cerrarlo" es un no-op) — se ignora en vez
                // de dejarla como error no controlado en la consola.
                dotNetRef.invokeMethodAsync('CerrarDesdeJs').catch(() => {});
            }
        };
        document.addEventListener('mousedown', handler, true);
        this._handlers.set(root, handler);
    },

    dispose(root) {
        const handler = this._handlers.get(root);
        if (handler) {
            document.removeEventListener('mousedown', handler, true);
            this._handlers.delete(root);
        }
    },

    scrollActiveIntoView(listEl, activeId) {
        if (!listEl) return;
        const el = document.getElementById(activeId);
        if (!el) return;
        const elTop = el.offsetTop, elBottom = elTop + el.offsetHeight;
        const viewTop = listEl.scrollTop, viewBottom = viewTop + listEl.clientHeight;
        if (elTop < viewTop) listEl.scrollTop = elTop;
        else if (elBottom > viewBottom) listEl.scrollTop = elBottom - listEl.clientHeight;
    },

    // Antes, esto ensanchaba el MudDialog contenedor un 35% mientras el listbox estaba
    // abierto — pero eso reacomodaba todo el grid del formulario (2 columnas) cada vez
    // que se abría CUALQUIER combo, generando un "salto"/distorsión visual del resto de
    // los campos. Ahora el listbox mismo puede crecer más ancho que el input (ver
    // ".aria-combo-listbox" en app.css: min-width:100% + width:max-content), así que ya
    // no hace falta tocar el diálogo — estos métodos quedan como no-op (se conservan para
    // no romper las llamadas desde AriaCombo.razor).
    marcarAbierto(root) {},

    marcarCerrado(root) {},

    // Reposiciona el listbox con position:fixed en coordenadas de VIEWPORT (no relativas
    // a ".aria-combo", que es como lo posiciona el CSS por defecto). Un position:fixed
    // solo queda contenido por un ancestro con transform/filter/perspective — ningún
    // overflow:auto/hidden normal (el de la grilla, el del cuerpo del diálogo, etc.) lo
    // recorta. Por eso, cuando el combo vive dentro de un contenedor con scroll propio,
    // la lista se veía "incompleta" (recortada en el borde del contenedor) aunque Items
    // tuviera todos los registros — no era un problema de datos, sino de recorte visual.
    posicionar(root, listEl) {
        if (!root || !listEl) return;

        const box = root.querySelector('.aria-combo-box');
        if (!box) return;
        const rect = box.getBoundingClientRect();

        // #app tiene transform:scale() (zoom propio de la app, ver appZoom.js) y se vuelve
        // el "containing block" de los descendientes position:fixed (spec CSS) — un fixed
        // con left/top en coordenadas reales de viewport, dentro de ese ancestro, se
        // renderiza multiplicado por el factor de escala y queda "desprendido" del combo.
        // appZoom.getScale() documenta exactamente esto: hay que DIVIDIR las coordenadas
        // por ese factor antes de asignarlas. Antes se directamente SALTABA el reposicionamiento
        // para cualquier nodo con un ancestro transformado (incluidos los diálogos, que
        // MudDialogProvider renderiza dentro de #app pese al comentario original de que
        // vivían afuera) — eso dejaba el listbox recortado por el overflow-y:auto del
        // propio diálogo en vez de arreglarlo. Ahora se compensa la escala en vez de huir.
        const scale = (window.appZoom && typeof window.appZoom.getScale === 'function')
            ? (window.appZoom.getScale() || 1) : 1;

        listEl.style.position = 'fixed';
        listEl.style.left = `${rect.left / scale}px`;
        listEl.style.minWidth = `${rect.width / scale}px`;
        listEl.style.top = `${(rect.bottom + 4) / scale}px`;
        listEl.style.bottom = 'auto';

        // Si no entra hacia abajo (queda debajo del borde inferior de la ventana), se abre
        // hacia arriba en su lugar — mismo patrón que cualquier combo/select estándar.
        // getBoundingClientRect ya devuelve coordenadas reales de pantalla (post-escala),
        // así que la comparación con window.innerHeight no necesita compensación.
        const listRect = listEl.getBoundingClientRect();
        if (listRect.bottom > window.innerHeight) {
            listEl.style.top = 'auto';
            listEl.style.bottom = `${(window.innerHeight - rect.top + 4) / scale}px`;
        }
    }
};
