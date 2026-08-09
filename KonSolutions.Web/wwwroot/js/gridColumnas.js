/*
    gridColumnas.js
    --------------------------------------------------------------------------
    Ancho de columnas ajustable con el mouse, y recordado entre sesiones, para
    TODAS las grillas de la app.

    Por qué en JS y no en Blazor: arrastrar el borde de una columna son decenas
    de eventos de mousemove por segundo. Pasarlos por el circuito de Blazor
    haría un render por cada uno y el arrastre se sentiría pegajoso. Acá se
    tocan los <col> directamente y Blazor ni se entera.

    Cómo identifica cada grilla: si el contenedor declara data-resizable-grid,
    ese es el nombre. Si no, se arma uno con la ruta de la página y los títulos
    de las columnas. Lo segundo es a propósito: así funciona en las grillas que
    ya existen sin tener que ir a marcarlas una por una, y si alguien agrega o
    saca una columna la clave cambia sola y no quedan anchos viejos aplicados a
    columnas que ya no son las mismas.

    Se guarda en localStorage y no en el servidor porque es una preferencia de
    cómo se ve la pantalla en ESTA máquina — el mismo usuario en un monitor
    distinto normalmente quiere otros anchos.
*/
(function () {
    'use strict';

    const PREFIJO = 'kon.grid.cols.';
    const MIN_PX = 40;          // por debajo de esto la columna deja de leerse
    const ZONA_AGARRE = 6;      // ancho en px de la franja sensible del borde
    const ALTO_MIN = 120;       // por debajo de esto no entra ninguna fila

    // ── Identidad de la grilla ──────────────────────────────────────────────
    function claveDe(tabla) {
        const marcado = tabla.closest('[data-resizable-grid]');
        if (marcado) return PREFIJO + marcado.getAttribute('data-resizable-grid');

        const titulos = [...tabla.querySelectorAll('thead th')]
            .map(th => (th.textContent || '').trim().slice(0, 20))
            .join('~');
        return PREFIJO + location.pathname + '|' + titulos;
    }

    function leer(clave) {
        try { return JSON.parse(localStorage.getItem(clave) || '{}'); }
        catch { return {}; }
    }

    function guardar(clave, anchos) {
        try { localStorage.setItem(clave, JSON.stringify(anchos)); }
        catch { /* modo privado o cuota llena: se pierde la preferencia, no la pantalla */ }
    }

    // ── Aplicar anchos ──────────────────────────────────────────────────────
    // Se escribe sobre el <th>, que es lo que MudDataGrid ya usa para su propio
    // Width. Un intento anterior inyectaba un <colgroup>: eso le desarmaba la
    // tabla a MudDataGrid y las columnas colapsaban.
    function aplicar(tabla, anchos) {
        const ths = tabla.querySelectorAll('thead th');
        ths.forEach((th, i) => {
            const guardado = anchos[i];
            if (!guardado) return;                 // sin ancho propio se respeta el de la pantalla
            const px = guardado + 'px';
            th.style.width = px;
            th.style.minWidth = px;
            th.style.maxWidth = px;
        });
    }

    // ── Arrastre ────────────────────────────────────────────────────────────
    function prepararCabecera(tabla) {
        const clave = claveDe(tabla);
        const anchos = leer(clave);
        aplicar(tabla, anchos);

        const ths = [...tabla.querySelectorAll('thead th')];
        ths.forEach((th, i) => {
            // Se revisa el agarre y no sólo la marca: MudDataGrid reconstruye el
            // thead al re-renderizar, y una celda puede conservar la marca pero
            // haber perdido su franja.
            if (th.dataset.konResize && th.querySelector(':scope > .kon-col-resize')) return;
            th.dataset.konResize = '1';
            // La cabecera fija usa position:sticky puesto por CSS. Pisarlo con
            // relative la despegaría, y sticky ya sirve de ancla para el absolute.
            const pos = getComputedStyle(th).position;
            if (pos === 'static') th.style.position = 'relative';

            const agarre = document.createElement('span');
            agarre.className = 'kon-col-resize';
            // Posicionado desde acá a propósito: si app.css quedara cacheado, un
            // span sin posicionar entraría en el flujo del encabezado y lo
            // descuadraría. El CSS sólo agrega el color y la marca.
            agarre.style.cssText =
                'position:absolute;top:0;right:-3px;height:100%;width:' + ZONA_AGARRE +
                'px;cursor:col-resize;user-select:none;z-index:20;';
            agarre.title = 'Arrastrar para cambiar el ancho · doble clic para restablecer';
            th.appendChild(agarre);

            agarre.addEventListener('mousedown', ev => {
                ev.preventDefault();
                ev.stopPropagation();   // que no dispare el ordenamiento de la columna

                const xInicial = ev.clientX;
                const anchoInicial = th.getBoundingClientRect().width;

                document.body.classList.add('kon-redimensionando');
                agarre.classList.add('activo');

                let ancho = anchoInicial;
                function mover(e) {
                    ancho = Math.max(MIN_PX, Math.round(anchoInicial + (e.clientX - xInicial)));
                    const px = ancho + 'px';
                    th.style.width = px;
                    th.style.minWidth = px;
                    th.style.maxWidth = px;
                }

                function soltar() {
                    document.removeEventListener('mousemove', mover);
                    document.removeEventListener('mouseup', soltar);
                    document.body.classList.remove('kon-redimensionando');
                    agarre.classList.remove('activo');

                    const guardados = leer(clave);
                    guardados[i] = ancho;
                    guardar(clave, guardados);
                }

                document.addEventListener('mousemove', mover);
                document.addEventListener('mouseup', soltar);
            });

            // Doble clic: vuelve al ancho que declaró la pantalla.
            agarre.addEventListener('dblclick', ev => {
                ev.preventDefault();
                ev.stopPropagation();
                const guardados = leer(clave);
                delete guardados[i];
                guardar(clave, guardados);
                th.style.width = th.style.minWidth = th.style.maxWidth = '';
                aplicar(tabla, guardados);
            });
        });
    }

    // ── Alto de la grilla ───────────────────────────────────────────────────
    // El proyecto ya traía el estilo de un tirador de alto (.grid-resize-handle) pero
    // ningún código que lo manejara. Se engancha acá, junto al de columnas, porque el
    // problema es el mismo: preferencias de tamaño que conviene recordar.
    function prepararAlto(caja) {
        const tirador = caja.querySelector(':scope > .grid-resize-handle');
        if (!tirador || tirador.dataset.konAlto) return;

        const cont = caja.querySelector('.mud-table-container');
        if (!cont) return;                       // la grilla todavía no se dibujó
        tirador.dataset.konAlto = '1';

        const clave = PREFIJO + 'alto.' + (caja.getAttribute('data-resizable-grid') || location.pathname);
        const guardado = parseInt(localStorage.getItem(clave) || '', 10);
        if (guardado > 0) cont.style.height = guardado + 'px';

        tirador.addEventListener('mousedown', ev => {
            ev.preventDefault();
            const yInicial = ev.clientY;
            const altoInicial = cont.getBoundingClientRect().height;
            let alto = altoInicial;

            document.body.style.cursor = 'row-resize';
            document.body.style.userSelect = 'none';
            tirador.classList.add('arrastrando');

            function mover(e) {
                alto = Math.max(ALTO_MIN, Math.round(altoInicial + (e.clientY - yInicial)));
                cont.style.height = alto + 'px';
            }
            function soltar() {
                document.removeEventListener('mousemove', mover);
                document.removeEventListener('mouseup', soltar);
                document.body.style.cursor = '';
                document.body.style.userSelect = '';
                tirador.classList.remove('arrastrando');
                try { localStorage.setItem(clave, String(alto)); } catch { }
            }
            document.addEventListener('mousemove', mover);
            document.addEventListener('mouseup', soltar);
        });

        // Doble clic: vuelve al alto que declaró la pantalla.
        tirador.addEventListener('dblclick', () => {
            try { localStorage.removeItem(clave); } catch { }
            cont.style.height = '';
        });
    }

    function repasar(raiz) {
        (raiz || document).querySelectorAll('[data-resizable-grid]').forEach(caja => {
            try { prepararAlto(caja); } catch { /* que una grilla no tumbe a las demás */ }
        });

        const tablas = (raiz || document).querySelectorAll('table.mud-table-root, table.sim-grid');
        tablas.forEach(t => {
            if (!t.querySelector('thead th')) return;   // todavía sin cabecera
            try { prepararCabecera(t); } catch { /* una grilla rota no puede tumbar las demás */ }
        });
    }

    // Las grillas aparecen y se re-renderizan solas: al cambiar de solapa, al
    // abrir un documento, al filtrar. Por eso se observa el DOM en vez de
    // enganchar una sola vez al cargar.
    let pendiente = null;
    const observador = new MutationObserver(() => {
        clearTimeout(pendiente);
        pendiente = setTimeout(() => repasar(document), 120);
    });

    function iniciar() {
        repasar(document);
        observador.observe(document.body, { childList: true, subtree: true });
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', iniciar);
    else iniciar();

    // Para poder limpiarlo desde la consola si alguna vez hace falta.
    window.konGridColumnas = {
        restablecerTodo() {
            Object.keys(localStorage)
                .filter(k => k.startsWith(PREFIJO))
                .forEach(k => localStorage.removeItem(k));
            location.reload();
        }
    };
})();
