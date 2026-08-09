// Al fallar el submit de un <EditForm> (OnInvalidSubmit), busca el primer campo marcado
// como "campo-invalido" (CampoTexto.razor / AriaCombo.razor lo agregan solos cuando son
// obligatorios y quedaron vacíos) y le hace scroll + foco, para que el usuario vea de
// inmediato cuál falta llenar sin tener que leer un mensaje genérico en otra parte.
window.validationFocus = {
    focusFirstInvalid(root) {
        if (!root) return;
        const el = root.querySelector(
            '.campo-invalido input, .campo-invalido textarea, .campo-invalido [role="combobox"]'
        );
        if (!el) return;
        el.scrollIntoView({ behavior: 'smooth', block: 'center' });
        try { el.focus({ preventScroll: true }); } catch { el.focus(); }
    }
};
