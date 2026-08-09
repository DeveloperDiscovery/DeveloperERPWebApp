// Dictado por voz (Web Speech API) — usado por SolicitudesComercialesMemoriasDialog.razor
// ("Ayuda Memoria") para completar el textarea de DES_MEMORIA hablando en vez de escribir.
// Todo el reconocimiento ocurre en el navegador (no se sube audio al servidor). Sigue el
// mismo patrón que ariaCombo.js: objeto global window.dictadoVoz, sin ES module.
window.dictadoVoz = {
    _recognition: null,
    _dotNetRef: null,

    // Devuelve false si el navegador no soporta Web Speech API (ej. Firefox de escritorio) —
    // el componente Blazor usa esto para deshabilitar el botón de micrófono con un tooltip.
    disponible() {
        return !!(window.SpeechRecognition || window.webkitSpeechRecognition);
    },

    iniciar(dotNetRef, idioma) {
        const Ctor = window.SpeechRecognition || window.webkitSpeechRecognition;
        if (!Ctor) {
            dotNetRef.invokeMethodAsync('OnDictadoError', 'Este navegador no soporta dictado por voz.');
            return;
        }
        // Si quedó una sesión previa viva (ej. el usuario cerró el diálogo sin soltar el
        // botón), se corta antes de arrancar una nueva.
        this.detener();

        const recognition = new Ctor();
        recognition.lang = idioma || 'es-PE';
        recognition.continuous = true;
        recognition.interimResults = false;

        recognition.onresult = (ev) => {
            let texto = '';
            for (let i = ev.resultIndex; i < ev.results.length; i++) {
                if (ev.results[i].isFinal) texto += ev.results[i][0].transcript;
            }
            if (texto.trim()) dotNetRef.invokeMethodAsync('OnDictadoTexto', texto.trim());
        };
        recognition.onerror = (ev) => {
            // 'no-speech' y 'aborted' son ruido esperado (silencio, o el propio usuario
            // soltando el botón) — no vale la pena mostrarlos como error al usuario.
            if (ev.error === 'no-speech' || ev.error === 'aborted') return;
            dotNetRef.invokeMethodAsync('OnDictadoError', 'No se pudo escuchar el micrófono (' + ev.error + ').');
        };
        recognition.onend = () => {
            // Puede terminar solo (silencio prolongado) sin que el usuario haya soltado el
            // botón — se avisa a Blazor para que el ícono vuelva a su estado normal.
            dotNetRef.invokeMethodAsync('OnDictadoFin');
        };

        this._recognition = recognition;
        this._dotNetRef = dotNetRef;
        try { recognition.start(); } catch { /* ya estaba iniciado */ }
    },

    detener() {
        if (this._recognition) {
            try { this._recognition.stop(); } catch { /* noop */ }
            this._recognition = null;
        }
    }
};
