// Crea una blob: URL a partir de bytes descargados con fetch autenticado (HttpClient),
// para poder mostrarlos en un <iframe> sin depender de rutas UNC ni de Mixed Content.
window.blobViewer = {
    _urls: [],
    crear: function (bytes, contentType) {
        const blob = new Blob([new Uint8Array(bytes)], { type: contentType || 'application/pdf' });
        const url = URL.createObjectURL(blob);
        this._urls.push(url);
        return url;
    },
    liberar: function (url) {
        try { URL.revokeObjectURL(url); } catch (e) { }
        this._urls = this._urls.filter(u => u !== url);
    },
    liberarTodo: function () {
        this._urls.forEach(u => { try { URL.revokeObjectURL(u); } catch (e) { } });
        this._urls = [];
    }
};
