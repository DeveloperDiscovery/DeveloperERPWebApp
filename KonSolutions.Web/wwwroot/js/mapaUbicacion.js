// mapaUbicacion.js — mapa de ubicación (EntidadesDireccionesDialog.razor) para elegir
// VAL_LATITUD/VAL_LONGITUD haciendo click. Usa Leaflet + tiles de OpenStreetMap (gratis,
// sin API key) en vez del SDK de Google Maps JavaScript, que exige una cuenta de Google
// Cloud con facturación habilitada que no tenemos configurada acá.
window.mapaUbicacion = {
    _mapas: new Map(),

    // Centro por defecto: Lima, Perú — se usa solo si la dirección todavía no tiene
    // Latitud/Longitud guardadas (alta nueva).
    _centroDefault: [-12.0464, -77.0428],

    init(mapEl, dotNetRef, lat, lng) {
        if (!mapEl || !window.L) return;
        this.dispose(mapEl);

        const centro = (lat != null && lng != null) ? [lat, lng] : this._centroDefault;
        const map = L.map(mapEl, { attributionControl: false }).setView(centro, (lat != null && lng != null) ? 16 : 6);
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            maxZoom: 19,
            attribution: '&copy; OpenStreetMap'
        }).addTo(map);

        let marker = null;
        if (lat != null && lng != null) {
            marker = L.marker([lat, lng], { draggable: true }).addTo(map);
            marker.on('dragend', () => {
                const p = marker.getLatLng();
                dotNetRef.invokeMethodAsync('OnMapaClick', p.lat, p.lng);
            });
        }

        map.on('click', (e) => {
            if (marker) marker.setLatLng(e.latlng);
            else {
                marker = L.marker(e.latlng, { draggable: true }).addTo(map);
                marker.on('dragend', () => {
                    const p = marker.getLatLng();
                    dotNetRef.invokeMethodAsync('OnMapaClick', p.lat, p.lng);
                });
                this._mapas.get(mapEl).marker = marker;
            }
            dotNetRef.invokeMethodAsync('OnMapaClick', e.latlng.lat, e.latlng.lng);
        });

        this._mapas.set(mapEl, { map, marker });

        // El mapa se crea dentro de un MudDialog que puede terminar de animar su tamaño
        // DESPUÉS de este init() — Leaflet mide el contenedor al crear el mapa, así que si
        // el diálogo todavía estaba animándose, el mapa queda con un tamaño/recorte
        // incorrecto hasta que se redimensiona la ventana. Se fuerza un invalidateSize()
        // corto después de que termine cualquier animación de apertura.
        setTimeout(() => map.invalidateSize(), 300);
    },

    // Mueve/crea el marcador desde C# (cuando el usuario edita Latitud/Longitud a mano
    // en los campos numéricos, en vez de hacer click en el mapa).
    actualizarMarcador(mapEl, lat, lng) {
        const entry = this._mapas.get(mapEl);
        if (!entry || lat == null || lng == null) return;
        const punto = [lat, lng];
        if (entry.marker) entry.marker.setLatLng(punto);
        else {
            entry.marker = L.marker(punto, { draggable: true }).addTo(entry.map);
        }
        entry.map.setView(punto, Math.max(entry.map.getZoom(), 16));
    },

    // Busca una dirección de texto (geocodificación) usando Nominatim (OpenStreetMap,
    // gratis, sin API key) y centra el mapa ahí — no mueve el marcador todavía, el usuario
    // confirma con un click. Devuelve null si no encontró nada.
    async buscarDireccion(mapEl, texto) {
        const entry = this._mapas.get(mapEl);
        if (!entry || !texto || !texto.trim()) return null;
        try {
            const url = `https://nominatim.openstreetmap.org/search?format=json&limit=1&q=${encodeURIComponent(texto)}`;
            const resp = await fetch(url, { headers: { 'Accept-Language': 'es' } });
            const datos = await resp.json();
            if (!datos || !datos.length) return null;
            const lat = parseFloat(datos[0].lat), lng = parseFloat(datos[0].lon);
            entry.map.setView([lat, lng], 16);
            return { lat, lng };
        } catch { return null; }
    },

    dispose(mapEl) {
        const entry = this._mapas.get(mapEl);
        if (entry) {
            entry.map.remove();
            this._mapas.delete(mapEl);
        }
    }
};