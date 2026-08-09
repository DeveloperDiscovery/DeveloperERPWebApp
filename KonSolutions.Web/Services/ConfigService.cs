using System.Text;
using System.Xml.Serialization;
using Blazored.LocalStorage;
using Microsoft.JSInterop;
using KONSolutions.Shared.Models.Config;
using KONSolutions.Shared.Models.Estandar;

namespace KONSolutions.Web.Services;

/// <summary>
/// Carga, guarda (en XML dentro de localStorage) y aplica la configuración de apariencia.
/// El XML permite exportar/importar la configuración como archivo.
/// </summary>
public class ConfigService
{
    private const string Key = "app_config_xml";
    private readonly ILocalStorageService _storage;
    private readonly IJSRuntime _js;

    public AppConfig Current { get; private set; } = new();

    /// <summary>Se dispara cada vez que se aplica la configuración (general o de usuario), para que
    /// componentes como MainLayout puedan refrescarse sin esperar a un nuevo login.</summary>
    public event Action? OnChange;

    public ConfigService(ILocalStorageService storage, IJSRuntime js)
    { _storage = storage; _js = js; }

    public async Task LoadAsync()
    {
        try
        {
            var xml = await _storage.GetItemAsStringAsync(Key);
            if (!string.IsNullOrWhiteSpace(xml))
            {
                Current = Deserializar(xml) ?? new AppConfig();
                // Evita DivideByZeroException en MudDataGrid cuando vienen 0 de BD
                if (Current.FilasPorPaginaUnica <= 0) Current.FilasPorPaginaUnica = 10;
                if (Current.FilasPorPaginaPadre <= 0) Current.FilasPorPaginaPadre = 10;
                if (Current.FilasPorPaginaHijo  <= 0) Current.FilasPorPaginaHijo  = 10;
            }
        }
        catch { Current = new AppConfig(); }
    }

    public async Task SaveAsync(AppConfig config)
    {
        Current = config;
        var xml = Serializar(config);
        await _storage.SetItemAsStringAsync(Key, xml);
        await AplicarAsync();
    }

    /// <summary>
    /// Aplica las preferencias propias del usuario (Configuración de Usuario): zoom web,
    /// minutos de bloqueo de pantalla (prevalece sobre la Configuración General) y fondo
    /// del workspace. No se guardan en el XML general — vienen de la API por usuario.
    /// </summary>
    public async Task AplicarUsuarioAsync(KONSolutions.Shared.Models.Estandar.ConfiguracionUsuario? cfgUsuario)
    {
        Current.ZoomWebUsuario        = cfgUsuario?.TAS_ZOOM_WEB;
        Current.BloqueoMinutosUsuario = cfgUsuario?.NUM_MINUTOS_LOCKOUT_SCREEN;
        Current.FondoWorkspaceBase64  = cfgUsuario?.IMG_FONDO_WORKSPACE is { Length: > 0 } img
            ? Convert.ToBase64String(img) : null;
        Current.EsZurdo               = cfgUsuario?.FLG_USUARIO_SURDO ?? false;
        Current.NotificacionesActivas = cfgUsuario?.FLG_NOTIFICACION_ACTIVA ?? true;
        Current.TecladoVirtualUsuario = cfgUsuario?.FLG_TECLADO_VIRTUAL ?? false;

        // Filas por página: la preferencia del usuario prevalece sobre la Configuración
        // General (mismo patrón que BloqueoMinutosUsuario), aplicando al mismo valor
        // para grillas únicas/padre/hijo — al usuario le interesa un solo número.
        if (cfgUsuario?.NUM_ROWS_PAGE is > 0)
        {
            Current.FilasPorPaginaUnica = cfgUsuario.NUM_ROWS_PAGE.Value;
            Current.FilasPorPaginaPadre = cfgUsuario.NUM_ROWS_PAGE.Value;
            Current.FilasPorPaginaHijo  = cfgUsuario.NUM_ROWS_PAGE.Value;
        }

        var zoom = Current.ZoomWebUsuario is > 0 ? Current.ZoomWebUsuario.Value : 100;
        try { await _js.InvokeVoidAsync("appZoom.set", zoom); } catch { }

        await AplicarAsync();
    }

    /// <summary>Aplica la configuración como variables CSS en el documento.</summary>
    public async Task AplicarAsync()
    {
        var c = Current;
        var css = new StringBuilder();
        css.Append($"--rust:{c.ColorPrimario};");
        if (c.DegradeBackgroundColor && !string.IsNullOrWhiteSpace(c.ColorFondo))
            css.Append($"--paper:linear-gradient(135deg,{c.ColorFondo},color-mix(in srgb,{c.ColorFondo} 40%,white));");
        else
            css.Append($"--paper:{c.ColorFondo};");
        css.Append($"--ink:{c.ColorTexto};");
        if (c.DegradeColorSecundario && !string.IsNullOrWhiteSpace(c.ColorSecundario))
            css.Append($"--secondary:linear-gradient(135deg,{c.ColorSecundario},color-mix(in srgb,{c.ColorSecundario} 40%,white));");
        else
            css.Append($"--secondary:{c.ColorSecundario};");
        css.Append($"--titulo-font:'{c.TituloFuente}';");
        css.Append($"--titulo-size:{c.TituloTamano}px;");
        css.Append($"--subtitulo-font:'{c.SubtituloFuente}';");
        css.Append($"--subtitulo-size:{c.SubtituloTamano}px;");
        css.Append($"--control-font:'{c.ControlFuente}';");
        css.Append($"--control-size:{c.ControlTamano}px;");
        // Iconos de grilla (punto 3)
        css.Append($"--icono-grilla-ancho:{c.IconoGrillaAncho}px;");
        css.Append($"--icono-grilla-alto:{c.IconoGrillaAlto}px;");
        if (!string.IsNullOrWhiteSpace(c.IconoAccionesColor)) css.Append($"--icono-acciones-color:{c.IconoAccionesColor};");
        // Colores de grilla (punto 8)
        css.Append($"--grilla-cab-fondo:{c.GrillaCabeceraFondo};");
        css.Append($"--grilla-cab-texto:{c.GrillaCabeceraTexto};");
        css.Append($"--grilla-fila-par:{c.GrillaFilaPar};");
        css.Append($"--grilla-fila-impar:{c.GrillaFilaImpar};");
        css.Append($"--grilla-texto:{c.GrillaTexto};");
        css.Append($"--grilla-lineas:{c.GrillaLineas};");
        if (c.GrillaFilaAlto is > 0)     css.Append($"--grilla-fila-alto:{c.GrillaFilaAlto}px;");
        if (c.GrillaFilaFontSize is > 0) css.Append($"--grilla-fila-fontsize:{c.GrillaFilaFontSize}px;");
        if (c.GrillaCabAlto is > 0)      css.Append($"--grilla-cab-alto:{c.GrillaCabAlto}px;");
        if (c.GrillaCabFontSize is > 0)  css.Append($"--grilla-cab-fontsize:{c.GrillaCabFontSize}px;");
        // Colores de tipografía (punto 14)
        css.Append($"--titulo-color:{c.TituloColor};");
        css.Append($"--subtitulo-color:{c.SubtituloColor};");
        css.Append($"--control-color:{c.ControlColor};");
        // Fila con foco (puntos 17 y 19)
        css.Append($"--foco-fondo:{c.FocoFondo};");
        css.Append($"--foco-texto:{c.FocoTexto};");
        css.Append($"--foco-fuente:'{c.FocoFuente}';");
        // v3: fuentes de filas y color de obligatorios
        css.Append($"--grilla-fila-fuente:'{c.GrillaFilaFuente}';");
        css.Append($"--grilla-fila-par-fuente:'{c.GrillaFilaParFuente}';");
        css.Append($"--grilla-fila-impar-fuente:'{c.GrillaFilaImparFuente}';");
        if (!string.IsNullOrWhiteSpace(c.FilaSinFocoFondo)) css.Append($"--fila-sinfoco-fondo:{c.FilaSinFocoFondo};");
        if (!string.IsNullOrWhiteSpace(c.FilaSinFocoTexto)) css.Append($"--fila-sinfoco-texto:{c.FilaSinFocoTexto};");
        css.Append($"--obligatorio-backcolor:{c.ObligatorioBackColor};");
        // Colores del entorno (menú, cabecera, sidebar, status-bar)
        if (!string.IsNullOrWhiteSpace(c.EntornoFondo))
        {
            var envBack = c.DegradeEntorno
                ? $"linear-gradient(to bottom,{c.EntornoFondo},color-mix(in srgb,{c.EntornoFondo} 40%,white))"
                : c.EntornoFondo;
            css.Append($"--env-back:{envBack};");
        }
        if (!string.IsNullOrWhiteSpace(c.EntornoTexto))  css.Append($"--env-fore:{c.EntornoTexto};");
        if (!string.IsNullOrWhiteSpace(c.EntornoFuente)) css.Append($"--env-font:'{c.EntornoFuente}';");
        // Colores del menú contextual (clic derecho)
        if (!string.IsNullOrWhiteSpace(c.MenuContextualFondo)) css.Append($"--menu-back:{c.MenuContextualFondo};");
        if (!string.IsNullOrWhiteSpace(c.MenuContextualTexto)) css.Append($"--menu-fore:{c.MenuContextualTexto};");
        // Colores del Login (independientes de los del entorno)
        if (!string.IsNullOrWhiteSpace(c.LoginFondo)) css.Append($"--login-back:{c.LoginFondo};");
        if (!string.IsNullOrWhiteSpace(c.LoginTexto)) css.Append($"--login-fore:{c.LoginTexto};");
        // Colores de los diálogos (crear/editar/ver/eliminar)
        if (!string.IsNullOrWhiteSpace(c.DialogFondo)) css.Append($"--dialog-back:{c.DialogFondo};");
        if (!string.IsNullOrWhiteSpace(c.DialogTexto)) css.Append($"--dialog-fore:{c.DialogTexto};");
        // Botones de diálogos (Cancelar/Crear/Modificar/Eliminar/Regresar)
        if (!string.IsNullOrWhiteSpace(c.BotonBackColor)) css.Append($"--boton-backcolor:{c.BotonBackColor};");
        if (!string.IsNullOrWhiteSpace(c.BotonForeColor)) css.Append($"--boton-forecolor:{c.BotonForeColor};");
        // Fondo del workspace (Configuración de Usuario)
        css.Append(!string.IsNullOrWhiteSpace(c.FondoWorkspaceBase64)
            ? $"--workspace-bg-image:url('data:image/png;base64,{c.FondoWorkspaceBase64}');"
            : "--workspace-bg-image:none;");
        try { await _js.InvokeVoidAsync("appConfig.apply", css.ToString()); } catch { }
        OnChange?.Invoke();
    }

    /// <summary>
    /// Mapea ESTANDAR.CONFIGURACION_GENERAL (BD) → AppConfig. Antes esto solo vivía inline
    /// dentro del botón "Guardar" de la pantalla Configuración General, así que si el
    /// navegador no tenía nada cacheado en localStorage (recién borrado, incógnito, otro
    /// equipo), la app arrancaba con los colores por defecto hasta que alguien entraba a
    /// esa pantalla y grababa manualmente. Extraído acá para poder aplicarlo también al
    /// arrancar la app (MainLayout), sin depender de esa visita manual.
    /// </summary>
    public static void MapearDesdeConfiguracionGeneral(KONSolutions.Shared.Models.Estandar.ConfiguracionGeneral cfg, AppConfig app)
    {
        app.NombreApp              = cfg.DES_NOMBRE_APLICACION;
        app.ColorPrimario          = cfg.DES_COLOR_PRIMARY;
        app.ColorFondo             = cfg.DES_BACKCOLOR;
        app.ColorTexto             = cfg.DES_FORECOLOR;
        app.ColorSecundario        = cfg.DES_COLOR_SECONDARY;
        app.TituloFuente           = cfg.DES_TYPOGRAPHY_TITLE_FONT;
        app.TituloTamano           = cfg.DES_TYPOGRAPHY_TITLE_SIZE;
        app.TituloColor            = cfg.DES_FORECOLOR_TITLE;
        app.SubtituloFuente        = cfg.DES_TYPOGRAPHY_SUBTITLE_FONT;
        app.SubtituloTamano        = cfg.NUM_TYPOGRAPHY_SUBTITLE_SIZE;
        app.SubtituloColor         = cfg.DES_FORECOLOR_SUBTITLE;
        app.ControlFuente          = cfg.DES_TYPOGRAPHY_CONTROLS_FONT;
        app.ControlTamano          = cfg.NUM_TYPOGRAPHY_CONTROLS_SIZE;
        app.RefrescoAutomatico     = cfg.FLG_ACTIVATE_REFRESH_AUTOMATIC;
        app.RefrescoIntervaloSegs  = cfg.NUM_SECONDS_REFRESH_GRID;
        app.ControlCambiosActivo   = cfg.FLG_CONTROL_CAMBIOS_ACTIVO;
        app.GrillaFilaAlto         = cfg.NUM_HIGH_ROW_GRID;
        app.GrillaFilaFontSize     = cfg.NUM_FONT_ROW_SIZE;
        app.GrillaCabAlto          = cfg.NUM_HIGH_HEAD_GRID;
        app.GrillaCabFontSize      = cfg.NUM_FONT_HEAD_SIZE;
        app.TecladoVirtual         = cfg.FLG_TECLADO_VIRTUAL;
        app.BloqueoActivo          = cfg.FLG_ACTIVAR_LOCKOUT_SCREEN;
        app.BloqueoMinutos         = cfg.NUM_MINUTOS_LOCKOUT_SCREEN;
        app.FaceTimeoutSegundos    = cfg.NUM_SECONDS_OUT_FACIAL_RECOGNITION;
        app.ObligatorioBackColor   = cfg.DES_BACKCOLOR_CONTROLS_REQUIRED;
        app.BotonBackColor         = cfg.DES_BUTTON_BACKCOLOR;
        app.BotonForeColor         = cfg.DES_BUTTON_FORECOLOR;
        app.IconoGrillaAncho       = cfg.NUM_ICONS_GRID_WIDTH_SIZE;
        app.IconoGrillaAlto        = cfg.NUM_ICONS_GRID_HEIGHT_SIZE;
        app.ImagenGrillaAncho      = cfg.NUM_IMAGES_GRID_WIDTH_SIZE;
        app.ImagenGrillaAlto       = cfg.NUM_IMAGES_GRID_HEIGHT_SIZE;
        app.ImagenMaxMB            = cfg.NUM_IMAGENES_MEGAS_UPLOAD;
        app.FilasPorPaginaUnica    = Math.Max(5, cfg.NUM_ROWS_SHOW_GRID_UNIQUE);
        app.FilasPorPaginaPadre    = Math.Max(5, cfg.NUM_ROWS_SHOW_GRID_FATHER);
        app.FilasPorPaginaHijo     = Math.Max(5, cfg.NUM_ROWS_SHOW_GRID_CHILDREN);
        app.FocoFondo              = cfg.DES_BACKCOLOR_ROWS_FOCUS;
        app.FocoTexto              = cfg.DES_FORECOLOR_ROWS_FOCUS;
        app.FocoFuente             = cfg.DES_TIPOGRAFIA_ROWS_FOCUS;
        app.GrillaFilaFuente       = cfg.DES_TIPOGRAFIA_GRID_ROWS;
        app.GrillaFilaParFuente    = cfg.DES_TIPOGRAFIA_GRID_ROWS_EVEN;
        app.GrillaFilaImparFuente  = cfg.DES_TIPOGRAFIA_GRID_ROWS_ODD;
        app.ModoColor              = cfg.DES_SKIN_COLOR;
        app.EntornoFondo           = cfg.DES_BACKCOLOR_ENVIRONMENT;
        app.EntornoTexto           = cfg.DES_FORECOLOR_ENVIRONMENT;
        app.EntornoFuente          = cfg.DES_TYPOGRAPHY_ENVIRONMENT_FONT;
        app.DegradeEntorno         = cfg.FLG_DEGRADE_ENVIRONMENT;
        app.DegradeBackgroundColor = cfg.FLG_DEGRADE_BACKGROUND_COLOR;
        app.DegradeColorSecundario = cfg.FLG_DEGRADE_SECUNDARY_COLOR;
        app.ModoTexto              = cfg.DES_MODE_TEXT;
        app.GrillaCabeceraFondo    = cfg.DES_BACKCOLOR_GRID_HEADER;
        app.GrillaCabeceraTexto    = cfg.DES_FORECOLOR_GRID_HEADER;
        app.GrillaTexto            = cfg.DES_FORECOLOR_GRID_ROWS;
        app.GrillaFilaPar          = cfg.DES_BACKCOLOR_GRID_ROWS_EVEN;
        app.GrillaFilaImpar        = cfg.DES_BACKCOLOR_GRID_ROWS_ODD;
        app.GrillaLineas           = cfg.DES_LINECOLOR_GRID_ROWS;
        app.MinCaracteresBusqueda  = Math.Max(1, cfg.NUM_CHARACTERS_MIO_SEARCH);
        app.MenuContextualFondo    = cfg.DES_BACKCOLOR_CONTEXTUALMENU;
        app.MenuContextualTexto    = cfg.DES_FORECOLOR_CONTEXTUALMENU;
        app.LoginFondo             = cfg.DES_BACKCOLOR_LOGIN;
        app.LoginTexto             = cfg.DES_FORECOLOR_LOGIN;
        app.DialogFondo            = cfg.DES_BACKCOLOR_DIALOG;
        app.DialogTexto            = cfg.DES_FORECOLOR_DIALOG;
        app.IconoAccionesColor     = cfg.DES_ICONCOLOR_ACTIONS;
        if (cfg.IMG_LOGO_APLICACION is { Length: > 0 })
            app.LogoApp = Convert.ToBase64String(cfg.IMG_LOGO_APLICACION);
    }

    public string ExportarXml() => Serializar(Current);

    public async Task<bool> ImportarXmlAsync(string xml)
    {
        var cfg = Deserializar(xml);
        if (cfg is null) return false;
        await SaveAsync(cfg);
        return true;
    }

    private static string Serializar(AppConfig config)
    {
        var serializer = new XmlSerializer(typeof(AppConfig));
        using var sw = new StringWriter();
        serializer.Serialize(sw, config);
        return sw.ToString();
    }

    private static AppConfig? Deserializar(string xml)
    {
        try
        {
            var serializer = new XmlSerializer(typeof(AppConfig));
            using var sr = new StringReader(xml);
            return serializer.Deserialize(sr) as AppConfig;
        }
        catch { return null; }
    }
}
