// ════════════════════════════════════════════════════════════════════════════
// Teclado Virtual global y adaptable al input.
//  - Detecta el foco en cualquier <input>/<textarea> de la app.
//  - Se adapta al tipo de campo: numérico -> teclado numérico; texto -> QWERTY.
//  - Escribe en el campo enfocado y dispara el evento 'input' para que el binding
//    de Blazor (@bind) tome el valor.
//  - Cada input elegible recibe un ícono ⌨ a la derecha; el ícono/teclado solo se
//    ACTIVAN si window.virtualKeyboard.setHabilitado(true) (lo decide Blazor según
//    los 3 flags: Config General AND Opción AND Config Usuario).
// ════════════════════════════════════════════════════════════════════════════
window.virtualKeyboard = (function () {
    let habilitado = false;
    let panel = null;
    let current = null;      // input enfocado
    let mode = 'abc';        // 'abc' | 'num'
    let shift = false;
    let observer = null;

    const COLOR_RUST = getComputedStyle(document.documentElement).getPropertyValue('--env-back') || '#b8472d';

    // ── ¿el campo es numérico? ────────────────────────────────────────────────
    function esNumerico(input) {
        const im = (input.getAttribute('inputmode') || '').toLowerCase();
        if (im === 'numeric' || im === 'decimal') return true;
        const tp = (input.getAttribute('type') || input.type || '').toLowerCase();
        if (tp === 'number' || tp === 'tel') return true;
        // MudNumericField: type=text pero suele traer data-nk / clase; heurística por patrón
        const pat = input.getAttribute('pattern') || '';
        if (/[\d]/.test(pat) && /^\[?\\?d/.test(pat)) return true;
        return false;
    }

    function elegible(t) {
        if (!t) return false;
        const tag = t.tagName;
        if (tag !== 'INPUT' && tag !== 'TEXTAREA') return false;
        if (t.readOnly || t.disabled) return false;
        const tp = (t.getAttribute('type') || t.type || '').toLowerCase();
        if (['checkbox', 'radio', 'range', 'color', 'file', 'hidden', 'submit', 'button', 'date', 'datetime-local', 'time'].includes(tp)) return false;
        if (t.dataset.vkIgnore === '1') return false;
        // Combos / selects / autocomplete: NO deben abrir el teclado, porque tapan el
        // panel de opciones y le roban el foco. MudSelect/MudAutocomplete marcan el input
        // con role=combobox / aria-haspopup / aria-autocomplete.
        const role = (t.getAttribute('role') || '').toLowerCase();
        if (role === 'combobox' || role === 'listbox' || role === 'searchbox') return false;
        if (t.hasAttribute('aria-haspopup') || t.hasAttribute('aria-autocomplete') ||
            t.getAttribute('aria-expanded') !== null || t.hasAttribute('aria-controls')) return false;
        if (t.classList.contains('mud-select-input') || t.closest('.mud-select, .mud-autocomplete, .mud-picker') !== null) return false;
        return true;
    }

    // ── Insertar / borrar respetando cursor y disparando 'input' ─────────────
    function escribir(ch) {
        if (!current) return;
        const s = current.selectionStart ?? current.value.length;
        const e = current.selectionEnd ?? current.value.length;
        current.value = current.value.slice(0, s) + ch + current.value.slice(e);
        const pos = s + ch.length;
        try { current.setSelectionRange(pos, pos); } catch (_) { }
        current.dispatchEvent(new Event('input', { bubbles: true }));
    }
    function borrar() {
        if (!current) return;
        const s = current.selectionStart ?? current.value.length;
        const e = current.selectionEnd ?? current.value.length;
        if (s === e && s > 0) {
            current.value = current.value.slice(0, s - 1) + current.value.slice(e);
            const pos = s - 1;
            try { current.setSelectionRange(pos, pos); } catch (_) { }
        } else {
            current.value = current.value.slice(0, s) + current.value.slice(e);
            try { current.setSelectionRange(s, s); } catch (_) { }
        }
        current.dispatchEvent(new Event('input', { bubbles: true }));
    }

    // ── Construcción del panel (una sola vez) ────────────────────────────────
    function crearPanel() {
        panel = document.createElement('div');
        panel.id = 'vk-panel';
        panel.style.cssText =
            'position:fixed;left:50%;bottom:0;transform:translateX(-50%);z-index:4000;' +
            'width:min(720px,98vw);background:linear-gradient(180deg,#faf7f2,#efeae3);' +
            'border:1px solid #d9d2c5;border-bottom:none;border-radius:14px 14px 0 0;' +
            'box-shadow:0 -6px 28px rgba(0,0,0,.22);padding:8px;display:none;user-select:none;';
        // Mantener el foco en el input al tocar el teclado (clave):
        panel.addEventListener('mousedown', e => e.preventDefault());
        document.body.appendChild(panel);
    }

    const FILAS_ABC = [
        ['1','2','3','4','5','6','7','8','9','0'],
        ['q','w','e','r','t','y','u','i','o','p'],
        ['a','s','d','f','g','h','j','k','l','ñ'],
        ['z','x','c','v','b','n','m']
    ];

    function tecla(txt, cls, onClick, flex) {
        const b = document.createElement('div');
        b.textContent = txt;
        b.style.cssText =
            'flex:' + (flex || 1) + ';min-width:0;text-align:center;padding:11px 0;border-radius:8px;' +
            'font-size:15px;font-weight:600;cursor:pointer;border:1px solid #d9d2c5;border-bottom:2px solid #cfc6b6;' +
            (cls === 'accent'
                ? 'background:' + COLOR_RUST + ';color:#fff;'
                : cls === 'mod'
                    ? 'background:#fff;color:#8a7f70;font-size:12px;font-weight:700;'
                    : 'background:#fdfbf7;color:#2a241d;');
        b.addEventListener('click', onClick);
        return b;
    }
    function fila() {
        const r = document.createElement('div');
        r.style.cssText = 'display:flex;gap:6px;margin-bottom:6px;justify-content:center;';
        return r;
    }

    const FILAS_SYM = [
        ['1','2','3','4','5','6','7','8','9','0'],
        ['!','@','#','$','%','&','*','(',')','-'],
        ['_','+','=','/','\\',':',';','"','\'','?'],
        ['¡','¿','€','£','°','^','~','|','<','>']
    ];

    function render() {
        panel.innerHTML = '';
        // Numérico = panel compacto; texto/símbolos = panel ancho.
        panel.style.width = (mode === 'num') ? 'min(300px,96vw)' : 'min(720px,98vw)';

        // cabecera
        const head = document.createElement('div');
        head.style.cssText = 'display:flex;align-items:center;gap:8px;padding:2px 6px 6px;color:#8a7f70;font-size:12px;';
        head.innerHTML = '<span>⌨️ Teclado</span><span style="margin-left:auto;font-family:monospace;font-size:11px;">' +
            (mode === 'num' ? '123' : mode === 'sym' ? '#+=' : 'ABC') + '</span>';
        panel.appendChild(head);

        if (mode === 'num') {
            // Teclado numérico compacto (teclas más chicas, panel angosto).
            const nums = [['7','8','9'],['4','5','6'],['1','2','3'],['.','0','⌫']];
            const permiteDecimal = current && !(current.getAttribute('data-vk-int') === '1');
            nums.forEach(rw => {
                const r = fila(); r.style.marginBottom = '5px';
                rw.forEach(k => {
                    let t;
                    if (k === '⌫') t = tecla('⌫', 'mod', borrar);
                    else if (k === '.') t = tecla('.', permiteDecimal ? '' : 'mod', () => { if (permiteDecimal) escribir('.'); });
                    else t = tecla(k, '', () => escribir(k));
                    t.style.padding = '10px 0'; t.style.fontSize = '16px';
                    r.appendChild(t);
                });
                panel.appendChild(r);
            });
            const r = fila();
            const bm = tecla('-', 'mod', () => escribir('-'), 1); bm.style.padding = '8px 0';
            const ba = tecla('ABC', 'mod', () => { mode = 'abc'; render(); }, 1); ba.style.padding = '8px 0';
            const bok = tecla('Aceptar', 'accent', ocultar, 2); bok.style.padding = '8px 0';
            r.appendChild(bm); r.appendChild(ba); r.appendChild(bok);
            panel.appendChild(r);
            return;
        }

        if (mode === 'sym') {
            FILAS_SYM.forEach(rw => {
                const r = fila();
                rw.forEach(k => r.appendChild(tecla(k, '', () => escribir(k))));
                panel.appendChild(r);
            });
            const r = fila();
            r.appendChild(tecla('ABC', 'mod', () => { mode = 'abc'; render(); }, 2));
            r.appendChild(tecla('123', 'mod', () => { mode = 'num'; render(); }, 1.5));
            r.appendChild(tecla('espacio', '', () => escribir(' '), 5));
            r.appendChild(tecla('⌫', 'mod', borrar, 1.5));
            r.appendChild(tecla('Aceptar', 'accent', ocultar, 2.4));
            panel.appendChild(r);
            return;
        }

        // QWERTY
        FILAS_ABC.forEach((rw, i) => {
            const r = fila();
            if (i === 3) r.appendChild(tecla('⇧', 'mod', () => { shift = !shift; render(); }, 1.6));
            rw.forEach(k => {
                const ch = (shift && i > 0) ? k.toUpperCase() : k;
                r.appendChild(tecla(ch, '', () => { escribir(ch); if (shift) { shift = false; render(); } }));
            });
            if (i === 3) r.appendChild(tecla('⌫', 'mod', borrar, 1.6));
            panel.appendChild(r);
        });
        const r = fila();
        r.appendChild(tecla('123', 'mod', () => { mode = 'num'; render(); }, 1.6));
        r.appendChild(tecla('#+=', 'mod', () => { mode = 'sym'; render(); }, 1.6));
        r.appendChild(tecla('espacio', '', () => escribir(' '), 5));
        r.appendChild(tecla('.', 'mod', () => escribir('.'), 1));
        r.appendChild(tecla('Aceptar', 'accent', ocultar, 2.4));
        panel.appendChild(r);
    }

    function mostrar(input) {
        if (!panel) crearPanel();
        current = input;
        mode = esNumerico(input) ? 'num' : 'abc';
        shift = false;
        render();
        panel.style.display = 'block';
        // Que el input no quede tapado por el teclado, SIN generar espacio extra al
        // final de la página: reservamos el alto del teclado con scroll-margin-bottom
        // y usamos block:'nearest' (solo scrollea si el campo está tapado, lo mínimo).
        setTimeout(() => {
            try {
                if (_prevInput && _prevInput !== input) _prevInput.style.scrollMarginBottom = '';
                _prevInput = input;
                input.style.scrollMarginBottom = (panel.offsetHeight + 14) + 'px';
                const r = input.getBoundingClientRect();
                if (r.bottom > panel.getBoundingClientRect().top)
                    input.scrollIntoView({ block: 'nearest', behavior: 'smooth' });
            } catch (_) { }
        }, 30);
    }
    let _prevInput = null;
    function ocultar() {
        if (panel) panel.style.display = 'none';
        if (_prevInput) { _prevInput.style.scrollMarginBottom = ''; _prevInput = null; }
        current = null;
    }

    // ── Ícono ⌨ pegado a cada input elegible ─────────────────────────────────
    function decorar(input) {
        if (input.dataset.vkIcon === '1') return;
        input.dataset.vkIcon = '1';
        const ic = document.createElement('span');
        ic.className = 'vk-ic';
        // ︎ (variation selector texto) fuerza el glifo monocromo en vez del
        // emoji a color — sin esto, el navegador ignora cualquier CSS "color" que
        // se le aplique porque el emoji trae su propia paleta fija.
        ic.textContent = '⌨︎';
        ic.title = 'Teclado virtual';
        // Cuelga del <body> y no del input, con position:fixed. Colgarlo del padre obligaba
        // a adivinar cuál de los envoltorios de MudBlazor termina siendo el offsetParent —y
        // no es siempre el mismo—, y a mezclar unidades: #app lleva transform:scale() para
        // el zoom propio de la app, así que los rectángulos del navegador vienen escalados
        // pero un top/left absoluto se interpreta sin escalar. Con fixed las dos cosas viven
        // en coordenadas de viewport y el desfase se va solo.
        ic.style.cssText =
            'position:fixed;z-index:1000;font-size:15px;line-height:' + ALTO_ICONO + 'px;' +
            'height:' + ALTO_ICONO + 'px;' +
            'cursor:pointer;opacity:' + (habilitado ? '.6' : '.25') + ';' +
            'pointer-events:' + (habilitado ? 'auto' : 'none') + ';';
        ic.addEventListener('mousedown', e => { e.preventDefault(); });
        ic.addEventListener('click', () => { if (habilitado) { input.focus(); mostrar(input); } });
        input._vkIcon = ic;
        document.body.appendChild(ic);
        posicionar(input);
    }
    const SEPARACION = 6;    // aire entre el ícono y el borde derecho del input

    // Todo se mide con el rectángulo real del input, en coordenadas de viewport, que es donde
    // también vive el ícono (position:fixed). Así el tamaño que se ve ya viene con el zoom de
    // la app aplicado y no hay ninguna conversión que pueda salir mal.
    function posicionar(input) {
        const ic = input._vkIcon; if (!ic) return;

        const r = input.getBoundingClientRect();
        // Sin caja no hay dónde ponerlo: pasa mientras el control todavía no se dibujó, o
        // cuando está en una solapa oculta. Se esconde y se reintenta en el próximo repaso.
        if (!r.width || !r.height) { ic.style.display = 'none'; return; }
        ic.style.display = '';

        const alto = ic.offsetHeight || 18;
        // En un textarea centrar no tiene sentido —quedaría en medio del texto—: ahí se
        // conserva el anclaje arriba, que es lo que hacía el código original.
        const centrado = input.tagName !== 'TEXTAREA' && r.height <= 60;
        const arriba = centrado ? (r.height - alto) / 2 : SEPARACION;

        ic.style.top = Math.round(r.top + arriba) + 'px';
        ic.style.left = Math.round(r.right - (ic.offsetWidth || 18) - SEPARACION) + 'px';
    }

    // Los íconos viven en el <body>, así que ya no se van cuando Blazor descarta el input:
    // hay que darlos de baja a mano o se acumulan flotando sobre la pantalla siguiente.
    const decorados = new Set();

    function refrescarIconos() {
        decorados.forEach(t => {
            if (t.isConnected && elegible(t)) return;
            if (t._vkIcon) t._vkIcon.remove();
            t._vkIcon = null;
            delete t.dataset.vkIcon;
            decorados.delete(t);
        });

        document.querySelectorAll('input, textarea').forEach(t => {
            if (!elegible(t)) return;
            decorar(t);
            decorados.add(t);
            if (t._vkIcon) {
                t._vkIcon.style.opacity = habilitado ? '.6' : '.25';
                t._vkIcon.style.pointerEvents = habilitado ? 'auto' : 'none';
                posicionar(t);
            }
        });
    }

    /// Reubicar sin volver a recorrer todo el DOM: es lo que hace falta al hacer scroll,
    /// donde nada cambió salvo dónde está cada input en la pantalla.
    function reubicarIconos() {
        decorados.forEach(t => { if (t.isConnected) posicionar(t); });
    }

    // ── Eventos globales ─────────────────────────────────────────────────────
    document.addEventListener('focusin', e => {
        if (!habilitado) return;
        if (elegible(e.target)) mostrar(e.target);
    });
    document.addEventListener('mousedown', e => {
        if (!panel || panel.style.display === 'none') return;
        if (panel.contains(e.target)) return;
        if (e.target && e.target.classList && e.target.classList.contains('vk-ic')) return;
        if (elegible(e.target)) return;   // pasar a otro input: se re-muestra
        ocultar();
    });

    return {
        init: function () {
            if (!panel) crearPanel();
            refrescarIconos();
            observer = new MutationObserver(() => refrescarIconos());
            observer.observe(document.body, { childList: true, subtree: true });
            // Redimensionar la ventana y hacer scroll reacomodan los inputs sin tocar el DOM,
            // así que el MutationObserver no se entera y los íconos quedarían donde estaban.
            // El scroll se escucha en captura porque el que importa suele ser el de un
            // contenedor interno (una grilla, un diálogo), y ese no burbujea hasta window.
            window.addEventListener('resize', refrescarIconos);
            window.addEventListener('scroll', reubicarIconos, true);
        },
        // Blazor llama esto con el resultado de: General AND Opción AND Usuario
        setHabilitado: function (b) {
            habilitado = !!b;
            if (!habilitado) ocultar();
            refrescarIconos();
        }
    };
})();
