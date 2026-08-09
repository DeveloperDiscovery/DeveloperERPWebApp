window.wsSplit = (function () {

    let _dir      = 'h';
    let _dragging = false;
    let _handle   = null;
    let _tooltip  = null;

    // ── Tooltip flotante que muestra % mientras se arrastra ──────────────
    function createTooltip() {
        if (_tooltip) return;
        _tooltip = document.createElement('div');
        _tooltip.style.cssText =
            'position:fixed;background:#16130f;color:#fff;font-size:.72rem;' +
            'font-family:monospace;padding:3px 8px;border-radius:4px;' +
            'pointer-events:none;z-index:9999;display:none;' +
            'border:1px solid rgba(255,255,255,.2);white-space:nowrap;';
        document.body.appendChild(_tooltip);
    }

    function showTooltip(e, pct) {
        if (!_tooltip) return;
        _tooltip.textContent = `${Math.round(pct)}% · ${Math.round(100 - pct)}%`;
        _tooltip.style.display = 'block';
        _tooltip.style.left = (e.clientX + 12) + 'px';
        _tooltip.style.top  = (e.clientY - 10) + 'px';
    }

    function hideTooltip() {
        if (_tooltip) _tooltip.style.display = 'none';
    }

    // ── Init ──────────────────────────────────────────────────────────────
    function init(direction) {
        _dir = direction || 'h';
        createTooltip();

        const handle = document.getElementById('ws-split-handle');
        if (!handle) return;

        // Si es el mismo elemento ya inicializado, no re-attachar
        if (handle === _handle) return;

        // Desconectar listeners del handle anterior
        if (_handle) {
            _handle.removeEventListener('mousedown', onMouseDown);
            _handle.removeEventListener('dblclick',  onDblClick);
            _handle.removeEventListener('touchstart', onTouchStart);
        }

        _handle = handle;
        _handle.addEventListener('mousedown', onMouseDown);
        _handle.addEventListener('dblclick',  onDblClick);
        _handle.addEventListener('touchstart', onTouchStart, { passive: false });
    }

    // ── Mouse ─────────────────────────────────────────────────────────────
    function onMouseDown(e) {
        e.preventDefault();
        _dragging = true;
        _handle.classList.add('ws-dragging');
        document.addEventListener('mousemove', onMouseMove);
        document.addEventListener('mouseup',   onMouseUp);
    }

    function onMouseMove(e) {
        if (!_dragging) return;
        const pct = calcRatio(e.clientX, e.clientY);
        applyRatio(pct);
        showTooltip(e, pct * 100);
    }

    function onMouseUp() {
        _dragging = false;
        if (_handle) _handle.classList.remove('ws-dragging');
        hideTooltip();
        document.removeEventListener('mousemove', onMouseMove);
        document.removeEventListener('mouseup',   onMouseUp);
    }

    // ── Touch ─────────────────────────────────────────────────────────────
    function onTouchStart(e) {
        e.preventDefault();
        _dragging = true;
        _handle.classList.add('ws-dragging');
        document.addEventListener('touchmove', onTouchMove, { passive: false });
        document.addEventListener('touchend',  onTouchEnd);
    }

    function onTouchMove(e) {
        if (!_dragging || !e.touches[0]) return;
        e.preventDefault();
        const t = e.touches[0];
        const pct = calcRatio(t.clientX, t.clientY);
        applyRatio(pct);
    }

    function onTouchEnd() {
        _dragging = false;
        if (_handle) _handle.classList.remove('ws-dragging');
        document.removeEventListener('touchmove', onTouchMove);
        document.removeEventListener('touchend',  onTouchEnd);
    }

    // ── Doble clic → igualar paneles ──────────────────────────────────────
    function onDblClick() {
        applyRatio(0.5);
    }

    // ── Helpers ───────────────────────────────────────────────────────────
    function calcRatio(clientX, clientY) {
        const container = document.getElementById('ws-split-container');
        if (!container) return 0.5;
        const rect = container.getBoundingClientRect();
        const raw  = _dir === 'v'
            ? (clientX - rect.left)  / rect.width
            : (clientY - rect.top)   / rect.height;
        return Math.min(Math.max(raw, 0.15), 0.85);
    }

    function applyRatio(ratio) {
        const panelA = document.getElementById('ws-panel-a');
        const panelB = document.getElementById('ws-panel-b');
        if (!panelA || !panelB) return;
        panelA.style.flex = `0 0 ${(ratio * 100).toFixed(2)}%`;
        panelB.style.flex = '1 1 0';
        // Notificar a grillas y componentes que el tamaño cambió
        window.dispatchEvent(new Event('resize'));
    }

    return { init };
})();
