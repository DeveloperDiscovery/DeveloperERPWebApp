// Editor de polígonos sobre el plano de un nivel (PlanoZonificacion.razor).
//
// A diferencia de ariaCombo.js / dictadoVoz.js, este NO se registra con un <script> en
// index.html: es un módulo ES que el componente carga con import() la primera vez que se
// usa. Así no hay que acordarse de tocar index.html al desplegar, y el archivo solo se
// descarga cuando alguien abre la pantalla del plano.
//
// Blazor entrega OffsetX/OffsetY relativos al elemento que recibió el evento, que no sirve
// acá: el <svg> se escala con CSS (width:100%) y además puede estar dentro de un contenedor
// con scroll o del zoom de la app. getScreenCTM() es la única forma confiable de pasar de
// coordenadas de pantalla al sistema del viewBox contemplando todas esas transformaciones.

// Devuelve la posición del puntero NORMALIZADA (0..1) respecto del viewBox del <svg>,
// como arreglo [x, y]: un arreglo se deserializa igual en cualquier versión de
// System.Text.Json, sin depender de cómo se llamen las propiedades del objeto.
// Se recorta a [0,1] para que un arrastre que sale del plano deje el vértice pegado
// al borde en vez de fugarse fuera de la imagen.
export function puntoNormalizado(svg, clientX, clientY) {
    if (!svg || typeof svg.getScreenCTM !== 'function') return [0, 0];
    const ctm = svg.getScreenCTM();
    if (!ctm) return [0, 0];

    let local;
    if (typeof svg.createSVGPoint === 'function') {
        const punto = svg.createSVGPoint();
        punto.x = clientX;
        punto.y = clientY;
        local = punto.matrixTransform(ctm.inverse());
    } else {
        // createSVGPoint está deprecado; DOMPoint es el reemplazo moderno.
        local = new DOMPoint(clientX, clientY).matrixTransform(ctm.inverse());
    }

    const caja = svg.viewBox && svg.viewBox.baseVal ? svg.viewBox.baseVal : null;
    const ancho = caja && caja.width ? caja.width : (svg.clientWidth || 1);
    const alto = caja && caja.height ? caja.height : (svg.clientHeight || 1);

    return [
        Math.min(1, Math.max(0, local.x / ancho)),
        Math.min(1, Math.max(0, local.y / alto))
    ];
}

// Mientras se arrastra un vértice el puntero se sale del SVG con facilidad (sobre todo al
// trabajar cerca de los bordes del plano). setPointerCapture redirige todos los eventos al
// elemento hasta soltar, así el arrastre no se corta a mitad de camino ni queda "pegado"
// porque el pointerup ocurrió fuera.
export function capturarPuntero(el, pointerId) {
    try { el?.setPointerCapture?.(pointerId); } catch { /* el puntero ya se soltó */ }
}

export function liberarPuntero(el, pointerId) {
    try {
        if (el?.hasPointerCapture?.(pointerId)) el.releasePointerCapture(pointerId);
    } catch { /* ya liberado */ }
}
