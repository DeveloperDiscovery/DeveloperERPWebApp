// campoTexto.js — soporte mínimo para CampoTexto.razor: cuando el Modo de Texto
// (Configuración General) fuerza MAYÚSCULAS/minúsculas, el valor que Blazor debe pintar
// difiere del que el usuario acaba de escribir (mismo largo, distinto case), así que
// MudBlazor reescribe el .value del <input>/<textarea> real del DOM — eso hace que el
// navegador reubique el cursor al final, como con cualquier asignación programática de
// .value. Como la transformación es 1 a 1 (no inserta/borra caracteres), la posición
// correcta del cursor es la MISMA de antes de la transformación — solo hay que guardarla
// antes y reponerla después de que Blazor termine de re-renderizar.
window.campoTexto = {
    obtenerCursor(wrap) {
        const el = wrap && wrap.querySelector('input, textarea');
        return el ? el.selectionStart : null;
    },
    restaurarCursor(wrap, pos) {
        const el = wrap && wrap.querySelector('input, textarea');
        if (el) { try { el.setSelectionRange(pos, pos); } catch { } }
    }
};
