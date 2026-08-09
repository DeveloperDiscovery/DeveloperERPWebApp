// Zoom propio de la app, implementado con transform:scale (no con la propiedad CSS
// "zoom", que en Chrome/Edge desalinea window.innerHeight vs. las coordenadas internas
// de los elementos, causando cortes/huecos según el nivel de zoom). Con transform:scale
// + compensación de tamaño en vw/vh, el resultado final SIEMPRE encaja exacto en la
// ventana real, sin necesidad de recalcular nada por JS al cambiar de tamaño (vw/vh ya
// se recalculan solos).
window.appZoom = {
    currentScale: 1, // otros scripts (gridResize) lo leen para compensar alturas asignadas por JS
    set: function (pct) {
        var appEl = document.getElementById('app');
        if (!appEl) return;
        var scale = pct / 100;
        this.currentScale = scale;
        if (scale === 1) {
            appEl.style.transform = '';
            appEl.style.width = '';
            appEl.style.height = '';
            appEl.style.transformOrigin = '';
        } else {
            appEl.style.transformOrigin = 'top left';
            appEl.style.width = (100 / scale) + 'vw';
            appEl.style.height = (100 / scale) + 'vh';
            appEl.style.transform = 'scale(' + scale + ')';
        }
    },
    // Para menús contextuales propios (position:fixed dentro de #app): sus coordenadas
    // hay que dividirlas entre este factor antes de asignarlas a left/top, porque un
    // ancestro con transform se vuelve el "containing block" de los descendientes fixed.
    getScale: function () { return this.currentScale; }
};

window.focusNextElement = function () {
    var focusable = Array.from(document.querySelectorAll(
        'input:not([disabled]), select:not([disabled]), textarea:not([disabled]), button:not([disabled])'
    )).filter(function (el) { return el.offsetHeight > 0 && el.offsetWidth > 0; });
    var idx = focusable.indexOf(document.activeElement);
    if (idx >= 0 && idx < focusable.length - 1) {
        focusable[idx + 1].focus();
    }
};
