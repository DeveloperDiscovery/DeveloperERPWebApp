// Drag and drop de las notas del Tablón de Post-its (ENTORNO.POSTITS).
// Delegación de eventos sobre el contenedor del tablón: funciona también con notas
// agregadas dinámicamente ("Nuevo Post-it") sin tener que re-adjuntar listeners.
window.postits = (function () {
    let dotNetRef = null;
    let dragging = null;      // elemento .postit siendo arrastrado
    let startX = 0, startY = 0, startLeft = 0, startTop = 0;
    let boardEl = null;

    function scale() { return (window.appZoom && window.appZoom.currentScale) || 1; }

    function onPointerDown(e) {
        const nota = e.target.closest('.postit');
        if (!nota || !boardEl.contains(nota)) return;
        // No iniciar arrastre si el clic fue en un control interactivo de la nota
        // (botón borrar, campos de texto).
        if (e.target.closest('.postit-no-drag')) return;

        dragging = nota;
        startX = e.clientX; startY = e.clientY;
        startLeft = parseInt(nota.style.left || '0', 10);
        startTop = parseInt(nota.style.top || '0', 10);
        nota.classList.add('arrastrando');
        document.body.style.userSelect = 'none';
        e.preventDefault();
    }

    function onPointerMove(e) {
        if (!dragging) return;
        const s = scale();
        const dx = (e.clientX - startX) / s;
        const dy = (e.clientY - startY) / s;
        const boardRect = boardEl.getBoundingClientRect();
        const maxLeft = Math.max(0, boardRect.width / s - dragging.offsetWidth);
        const maxTop = Math.max(0, boardRect.height / s - dragging.offsetHeight);
        const left = Math.min(maxLeft, Math.max(0, startLeft + dx));
        const top = Math.min(maxTop, Math.max(0, startTop + dy));
        dragging.style.left = left + 'px';
        dragging.style.top = top + 'px';
    }

    function onPointerUp() {
        if (!dragging) return;
        const nota = dragging;
        dragging = null;
        nota.classList.remove('arrastrando');
        document.body.style.userSelect = '';
        const left = parseInt(nota.style.left || '0', 10);
        const top = parseInt(nota.style.top || '0', 10);
        const num = nota.getAttribute('data-postit');
        if (dotNetRef && num) {
            dotNetRef.invokeMethodAsync('GuardarPosicion', parseInt(num, 10), left, top);
        }
    }

    return {
        init: function (boardElement, dotNetReference) {
            boardEl = boardElement;
            dotNetRef = dotNetReference;
            boardEl.addEventListener('mousedown', onPointerDown);
            document.addEventListener('mousemove', onPointerMove);
            document.addEventListener('mouseup', onPointerUp);
        },
        dispose: function () {
            if (!boardEl) return;
            boardEl.removeEventListener('mousedown', onPointerDown);
            document.removeEventListener('mousemove', onPointerMove);
            document.removeEventListener('mouseup', onPointerUp);
            boardEl = null; dotNetRef = null; dragging = null;
        }
    };
})();
