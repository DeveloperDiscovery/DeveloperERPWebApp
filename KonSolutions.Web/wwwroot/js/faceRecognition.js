// face-api.js interop for Blazor
// Requires face-api.js loaded before this script.
// Models are loaded from /models/ folder in wwwroot.
window.faceRec = (function () {
    let _stream      = null;
    let _initialized = false;

    // ── Liveness: detección de parpadeo con calibración dinámica ─────────────
    // Usa el modelo COMPLETO de landmarks (faceLandmark68Net) para mayor precisión
    // en los puntos del ojo, especialmente con lentes.
    // El umbral EAR se calibra automáticamente midiendo el EAR con ojos abiertos.
    const EAR_CONSEC     = 1;    // frames bajo umbral para contar parpadeo
    const CALIB_FRAMES   = 25;   // muestras para calibrar (≈3 s a 8 fps)
    const CALIB_RATIO    = 0.72; // umbral = EAR_base × ratio
    const EAR_FALLBACK   = 0.20; // umbral fijo si calibración falla

    let _earFrameCount  = 0;
    let _blinkCount     = 0;
    let _earThreshold   = EAR_FALLBACK;
    let _calibSamples   = [];
    let _calibDone      = false;
    let _prevEyesClosed = false; // edge-trigger: contar al abrir, no al cerrar

    function resetBlinks() {
        _earFrameCount  = 0;
        _blinkCount     = 0;
        _calibSamples   = [];
        _calibDone      = false;
        _earThreshold   = EAR_FALLBACK;
        _prevEyesClosed = false;
    }

    function getBlinkCount() { return _blinkCount; }

    // EAR para un ojo (6 puntos landmark)
    function ear(pts) {
        const d = (a, b) => Math.hypot(a.x - b.x, a.y - b.y);
        return (d(pts[1], pts[5]) + d(pts[2], pts[4])) / (2.0 * d(pts[0], pts[3]));
    }

    function calcEAR(landmarks) {
        const pts = landmarks.positions;
        return (ear(pts.slice(36, 42)) + ear(pts.slice(42, 48))) / 2.0;
    }

    // Acumula muestras con ojos abiertos hasta CALIB_FRAMES, luego fija el umbral.
    function calibrar(earVal) {
        if (_calibDone) return;
        // Solo muestras donde el EAR es razonablemente estable (ojos abiertos)
        if (earVal > 0.08 && earVal < 0.50) _calibSamples.push(earVal);
        if (_calibSamples.length >= CALIB_FRAMES) {
            const sorted  = [..._calibSamples].sort((a, b) => a - b);
            // Percentil 60 para evitar outliers bajos (mini-parpadeos durante calibración)
            const p60     = sorted[Math.floor(sorted.length * 0.6)];
            _earThreshold = p60 * CALIB_RATIO;
            _calibDone    = true;
            console.log(`faceRec calibrado: EAR p60=${p60.toFixed(3)}, umbral=${_earThreshold.toFixed(3)}`);
        }
    }

    function processBlink(landmarks) {
        const earVal     = calcEAR(landmarks);
        calibrar(earVal);

        const eyesClosed = earVal < _earThreshold;

        // Edge-trigger: registrar parpadeo cuando los ojos se ABREN tras estar cerrados
        if (_prevEyesClosed && !eyesClosed) {
            if (_earFrameCount >= EAR_CONSEC) _blinkCount++;
            _earFrameCount = 0;
        } else if (eyesClosed) {
            _earFrameCount++;
        }
        _prevEyesClosed = eyesClosed;

        return { earValue: earVal, eyesClosed, blinkCount: _blinkCount, calibDone: _calibDone };
    }
    // ─────────────────────────────────────────────────────────────────────────

    async function init() {
        if (_initialized) return true;
        try {
            const local = '/models';
            const cdn   = 'https://raw.githubusercontent.com/justadudewhohacks/face-api.js/master/weights';
            let base = local;
            try {
                const probe = await fetch(local + '/tiny_face_detector_model-weights_manifest.json', { method: 'HEAD' });
                if (!probe.ok) base = cdn;
            } catch { base = cdn; }

            // Cargamos AMBOS modelos de landmarks:
            // - Tiny: para captureDescriptor (rápido, reconocimiento)
            // - Completo (68Net): para detectFrame (preciso, parpadeo con lentes)
            await Promise.all([
                faceapi.nets.tinyFaceDetector.loadFromUri(base),
                faceapi.nets.faceLandmark68TinyNet.loadFromUri(base),
                faceapi.nets.faceLandmark68Net.loadFromUri(base),
                faceapi.nets.faceRecognitionNet.loadFromUri(base)
            ]);
            _initialized = true;
            return true;
        } catch (e) {
            console.error('faceRec.init', e);
            return false;
        }
    }

    async function startCamera(videoId) {
        try {
            _stream = await navigator.mediaDevices.getUserMedia({ video: { width: 320, height: 240, facingMode: 'user' } });
            const video = document.getElementById(videoId);
            if (!video) return false;
            video.srcObject = _stream;
            await video.play();
            resetBlinks();
            return true;
        } catch (e) {
            console.error('faceRec.startCamera', e);
            return false;
        }
    }

    function stopCamera() {
        if (_stream) {
            _stream.getTracks().forEach(t => t.stop());
            _stream = null;
        }
        resetBlinks();
    }

    // captureDescriptor usa modelo TINY (rápido, suficiente para reconocimiento)
    async function captureDescriptor(videoId) {
        const video = document.getElementById(videoId);
        if (!video) return null;
        try {
            const detection = await faceapi
                .detectSingleFace(video, new faceapi.TinyFaceDetectorOptions({ scoreThreshold: 0.5 }))
                .withFaceLandmarks(true)       // true = tiny landmarks
                .withFaceDescriptor();
            if (!detection) return null;
            return Array.from(detection.descriptor);
        } catch (e) {
            console.error('faceRec.captureDescriptor', e);
            return null;
        }
    }

    // detectFrame usa modelo COMPLETO para mayor precisión en puntos del ojo
    async function detectFrame(videoId) {
        const video = document.getElementById(videoId);
        if (!video) return { faceVisible: false, eyesClosed: false, blinkCount: _blinkCount, earValue: 0, calibDone: _calibDone };
        try {
            const det = await faceapi
                .detectSingleFace(video, new faceapi.TinyFaceDetectorOptions({ scoreThreshold: 0.4 }))
                .withFaceLandmarks();          // sin parámetro = modelo COMPLETO (68Net)

            if (!det) return { faceVisible: false, eyesClosed: false, blinkCount: _blinkCount, earValue: 0, calibDone: _calibDone };

            const blink = processBlink(det.landmarks);
            return {
                faceVisible: true,
                eyesClosed:  blink.eyesClosed,
                blinkCount:  blink.blinkCount,
                earValue:    blink.earValue,
                calibDone:   blink.calibDone
            };
        } catch {
            return { faceVisible: false, eyesClosed: false, blinkCount: _blinkCount, earValue: 0, calibDone: _calibDone };
        }
    }

    async function isFaceVisible(videoId) {
        const video = document.getElementById(videoId);
        if (!video) return false;
        try {
            const det = await faceapi.detectSingleFace(video, new faceapi.TinyFaceDetectorOptions({ scoreThreshold: 0.4 }));
            return det != null;
        } catch { return false; }
    }

    return { init, startCamera, stopCamera, captureDescriptor, isFaceVisible, detectFrame, getBlinkCount, resetBlinks };
})();
