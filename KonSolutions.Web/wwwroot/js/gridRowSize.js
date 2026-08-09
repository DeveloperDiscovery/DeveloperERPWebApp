// Fuerza el alto de fila/cabecera y el tamaño de fuente de fila/cabecera de TODAS las
// grillas (MudDataGrid), leyendo las variables CSS que ya aplica appConfig.apply():
//   --grilla-fila-alto, --grilla-fila-fontsize, --grilla-cab-alto, --grilla-cab-fontsize
//
// Por qué existe: MudBlazor fija su propio alto de fila (modo Dense) con una prioridad
// que ni el CSS más específico con !important logra vencer (probablemente vía JS/inline
// propio que se reaplica en cada render). La única forma confiable de ganarle es
// escribir el estilo directamente sobre cada elemento ya renderizado, en cada mutación
// del DOM (nuevas pestañas, diálogos, filas que entran/salen al filtrar/paginar).
//
// "height" en una fila/celda de tabla es SOLO un mínimo: si el contenido (íconos de
// acción, padding) necesita más espacio del pedido, la fila crece igual sin importar
// cuántos !important se pongan — el navegador nunca corta contenido para achicar una
// fila. Por eso, para valores chicos de alto, además hay que achicar el padding interno
// y el tamaño de los íconos, o el alto pedido nunca se ve reflejado.
window.gridRowSize = {
    _observer: null,

    _leerVars: function () {
        var cs = getComputedStyle(document.documentElement);
        var val = function (name) { var v = cs.getPropertyValue(name).trim(); return v || null; };
        return {
            filaAlto: val('--grilla-fila-alto'),
            filaFont: val('--grilla-fila-fontsize'),
            cabAlto:  val('--grilla-cab-alto'),
            cabFont:  val('--grilla-cab-fontsize')
        };
    },

    // Padding vertical e ícono que caben dentro de un alto dado (heurística simple:
    // deja ~4px de aire arriba/abajo del ícono, con un piso de 2px de padding y
    // 12px de ícono para que no desaparezca del todo).
    // Padding vertical mínimo (deja el ícono lo más pegado posible al alto pedido) —
    // solo 1-2px de aire, el ícono se lleva casi todo el alto disponible.
    _encajar: function (altoPx) {
        var alto = parseFloat(altoPx);
        if (!alto || isNaN(alto)) return null;
        var padding = Math.max(0, Math.min(2, Math.floor((alto - 16) / 6)));
        var icono   = Math.max(12, Math.min(alto - (padding * 2), 32));
        return { padding: padding, icono: icono };
    },

    aplicar: function () {
        var v = this._leerVars();
        if (!v.filaAlto && !v.filaFont && !v.cabAlto && !v.cabFont) return;

        var cab  = v.cabAlto  ? this._encajar(v.cabAlto)  : null;
        var fila = v.filaAlto ? this._encajar(v.filaAlto) : null;

        // Sin exigir ancestro .mud-table-root: con FixedHeader, MudBlazor puede clonar
        // la cabecera en un contenedor sticky separado que igual usa las clases
        // .mud-table-head/.mud-table-cell pero queda fuera de .mud-table-root.
        document.querySelectorAll('.mud-table-head th, .mud-table-head .mud-table-cell, .mud-table-head tr').forEach(function (el) {
            if (v.cabAlto) {
                el.style.setProperty('height', v.cabAlto, 'important');
                el.style.setProperty('min-height', v.cabAlto, 'important');
            }
            if (v.cabFont) el.style.setProperty('font-size', v.cabFont, 'important');
            if (cab) {
                el.style.setProperty('padding-top', cab.padding + 'px', 'important');
                el.style.setProperty('padding-bottom', cab.padding + 'px', 'important');
                el.querySelectorAll('.mud-icon-root, svg').forEach(function (ic) {
                    ic.style.setProperty('width', cab.icono + 'px', 'important');
                    ic.style.setProperty('height', cab.icono + 'px', 'important');
                });
            }
        });
        document.querySelectorAll('.mud-table-root .mud-table-body .mud-table-row').forEach(function (el) {
            if (v.filaAlto) {
                el.style.setProperty('height', v.filaAlto, 'important');
                el.style.setProperty('min-height', v.filaAlto, 'important');
            }
        });
        document.querySelectorAll('.mud-table-root .mud-table-body .mud-table-cell').forEach(function (el) {
            if (v.filaAlto) {
                el.style.setProperty('height', v.filaAlto, 'important');
                el.style.setProperty('min-height', v.filaAlto, 'important');
            }
            if (v.filaFont) el.style.setProperty('font-size', v.filaFont, 'important');
            if (fila) {
                el.style.setProperty('padding-top', fila.padding + 'px', 'important');
                el.style.setProperty('padding-bottom', fila.padding + 'px', 'important');
                el.querySelectorAll('.mud-icon-root, svg, img').forEach(function (ic) {
                    ic.style.setProperty('width', fila.icono + 'px', 'important');
                    ic.style.setProperty('height', fila.icono + 'px', 'important');
                });
            }
        });
    },

    init: function () {
        var self = this;
        self.aplicar();
        if (self._observer) return;
        self._observer = new MutationObserver(function () {
            clearTimeout(self._debounce);
            self._debounce = setTimeout(function () { self.aplicar(); }, 50);
        });
        self._observer.observe(document.body, { childList: true, subtree: true });
    }
};
