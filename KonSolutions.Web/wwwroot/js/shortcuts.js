window.shortcuts = {
    _ref: null,
    iniciar: function (dotnetRef) {
        this._ref = dotnetRef;
        document.addEventListener('keydown', this._handler.bind(this), true);
    },
    _handler: function (e) {
        if (!this._ref) return;
        var enCampo = e.target && (e.target.tagName === 'INPUT' || e.target.tagName === 'TEXTAREA' || e.target.isContentEditable);
        var ctrl = e.ctrlKey || e.metaKey;

        // Ctrl+K siempre se captura (aunque el foco esté en un campo)
        if (ctrl && e.key.toLowerCase() === 'k') {
            e.preventDefault();
            this._ref.invokeMethodAsync('OnAtajoTeclado', 'ctrl+k');
            return;
        }
        if (ctrl && e.key.toLowerCase() === 'w') {
            e.preventDefault();
            this._ref.invokeMethodAsync('OnAtajoTeclado', 'ctrl+w');
            return;
        }
        if (e.key === 'Escape') {
            this._ref.invokeMethodAsync('OnAtajoTeclado', 'esc');
            return;
        }
        if (enCampo) return; // el resto de atajos no interfieren con la edición de texto

        if (ctrl && e.key.toLowerCase() === 'n') {
            e.preventDefault();
            this._ref.invokeMethodAsync('OnAtajoTeclado', 'ctrl+n');
        } else if (ctrl && e.key.toLowerCase() === 's') {
            e.preventDefault();
            this._ref.invokeMethodAsync('OnAtajoTeclado', 'ctrl+s');
        } else if (e.key === 'Delete') {
            this._ref.invokeMethodAsync('OnAtajoTeclado', 'delete');
        } else if (e.key === 'F2') {
            e.preventDefault();
            this._ref.invokeMethodAsync('OnAtajoTeclado', 'f2');
        }
    },
    focusSidebarSearch: function () {
        var el = document.getElementById('sidebar-search-input');
        if (el) el.focus();
    }
};
