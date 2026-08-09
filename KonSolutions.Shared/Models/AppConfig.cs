using System.Xml.Serialization;

namespace KONSolutions.Shared.Models.Config;

/// <summary>
/// Configuración de apariencia de la aplicación. Se serializa a XML.
/// Permite cambiar colores y tipografía de títulos, subtítulos, textbox, labels y combos.
/// </summary>
[XmlRoot("AppConfig")]
public class AppConfig
{
    // Colores de pantalla
    public string ColorPrimario   { get; set; } = "#b8472d"; // rust
    public string ColorFondo      { get; set; } = "#efe9df"; // paper
    public string ColorTexto      { get; set; } = "#16130f"; // ink
    public string ColorSecundario { get; set; } = "#6b6358";

    // Tipografía de TÍTULOS
    public string TituloFuente   { get; set; } = "Archivo";
    public int    TituloTamano   { get; set; } = 32;

    // Tipografía de SUBTÍTULOS
    public string SubtituloFuente { get; set; } = "Archivo";
    public int    SubtituloTamano { get; set; } = 18;

    // Tipografía COMÚN para textbox, labels y combos
    public string ControlFuente { get; set; } = "Spline Sans Mono";
    public int    ControlTamano { get; set; } = 14;

    // Refresco automático de grillas (control de cambios)
    public bool RefrescoAutomatico    { get; set; } = true;
    public int  RefrescoIntervaloSegs { get; set; } = 10;
    // Activa/desactiva el sondeo de Control de Cambios (ControlCambiosClientService) —
    // FLG_CONTROL_CAMBIOS_ACTIVO. Independiente de RefrescoAutomatico: ambos deben
    // estar en true para que arranque el sondeo.
    public bool ControlCambiosActivo  { get; set; } = true;

    // Identidad de la app (se guarda en localStorage como el resto de la config)
    public string  NombreApp    { get; set; } = "";
    public string? LogoApp      { get; set; }  // Logo aplicación en base64 (PNG)
    // Mínimo de caracteres antes de disparar la búsqueda contra la BD, en TODAS las
    // pantallas que usan BarraBusqueda.razor — NUM_CHARACTERS_MIO_SEARCH.
    public int     MinCaracteresBusqueda { get; set; } = 3;
    public string  NombreEmpresa { get; set; } = "";
    public string? LogoEmpresa   { get; set; }  // Logo empresa en base64 (PNG)

    // Comportamiento de mayúsculas/minúsculas en los textbox:
    // "normal" = como se escribe, "upper" = MAYÚSCULAS, "lower" = minúsculas
    public string ModoTexto { get; set; } = "normal";

    // ── Punto 3: tamaño de los iconos de acción en las grillas (px) ──
    public int IconoGrillaAncho { get; set; } = 20;
    public int IconoGrillaAlto  { get; set; } = 20;

    // ── Punto 4: tamaño de las IMÁGENES (campos IMG_) en las grillas (px) ──
    public int ImagenGrillaAncho { get; set; } = 40;
    public int ImagenGrillaAlto  { get; set; } = 40;

    // ── Punto 5: tamaño máximo de imágenes a cargar (MB), global ──
    public int ImagenMaxMB { get; set; } = 2;

    // ── Punto 7: modo de color del entorno: "claro" u "oscuro" ──
    public string ModoColor { get; set; } = "claro";

    // ── Colores del entorno (menú, cabecera, sidebar, status-bar) ──
    public string EntornoFondo            { get; set; } = "#1a1714";
    public string EntornoTexto            { get; set; } = "#ffffff";
    public string EntornoFuente           { get; set; } = "Archivo";
    public bool   DegradeEntorno          { get; set; }
    public bool   DegradeBackgroundColor  { get; set; }
    public bool   DegradeColorSecundario  { get; set; }

    // ── Punto 8: colores de la grilla (adicionales a los colores generales) ──
    public string GrillaCabeceraFondo { get; set; } = "#2b2622"; // fondo de cabecera
    public string GrillaCabeceraTexto { get; set; } = "#ffffff"; // texto de cabecera
    public string GrillaFilaPar       { get; set; } = "#ffffff"; // fila par (even)
    public string GrillaFilaImpar     { get; set; } = "#f5f1ea"; // fila impar (odd)
    public string GrillaTexto         { get; set; } = "#16130f"; // texto de las filas
    public string GrillaLineas        { get; set; } = "#d9d2c5"; // líneas/bordes

    // ── Alto de fila/cabecera y tamaño de fuente de fila/cabecera de las grillas ──
    // Independientes de --control-size: si son null se usa el valor por defecto (auto).
    public int? GrillaFilaAlto     { get; set; }
    public int? GrillaFilaFontSize { get; set; }
    public int? GrillaCabAlto      { get; set; }
    public int? GrillaCabFontSize  { get; set; }

    // ── Punto 10: filas por página en las grillas (únicas, padre, hijo) ──
    public int FilasPorPaginaUnica { get; set; } = 10;
    public int FilasPorPaginaPadre { get; set; } = 10;
    public int FilasPorPaginaHijo  { get; set; } = 10;

    // ── Punto 14: colores de la tipografía (títulos, subtítulos, controles) ──
    public string TituloColor    { get; set; } = "#16130f";
    public string SubtituloColor { get; set; } = "#6b6358";
    public string ControlColor   { get; set; } = "#16130f";

    // ── Puntos 17 y 19: color y fuente de la fila con foco / seleccionada ──
    public string FocoFondo  { get; set; } = "#b8472d"; // fondo de la fila con foco
    public string FocoTexto  { get; set; } = "#ffffff"; // texto de la fila con foco
    public string FocoFuente { get; set; } = "Spline Sans Mono"; // fuente de la fila con foco

    // ── v3: fuente de las filas de grilla (con foco, sin foco, par, impar) ──
    public string GrillaFilaFuente { get; set; } = "Archivo";   // fuente de TODAS las filas
    public string FilaSinFocoFondo { get; set; } = "";          // color fondo filas sin foco (vacío = usa par/impar)
    public string FilaSinFocoTexto { get; set; } = "";          // color texto filas sin foco (vacío = usa grilla-texto)
    public string GrillaFilaParFuente   { get; set; } = "Archivo"; // fuente fila par
    public string GrillaFilaImparFuente { get; set; } = "Archivo"; // fuente fila impar

    // ── v3: color de fondo de campos obligatorios (configurable) ──
    public string ObligatorioBackColor { get; set; } = "#fff3e0"; // backcolor de campos requeridos

    // ── Punto 15: bloqueo por inactividad ──
    public bool BloqueoActivo       { get; set; } = false; // activar bloqueo automático
    public int  BloqueoMinutos      { get; set; } = 5;     // minutos de inactividad antes de bloquear

    // ── Botones de diálogos (Cancelar/Crear/Modificar/Eliminar/Regresar) ──
    public string BotonBackColor { get; set; } = "";  // vacío = usa el color por defecto de MudBlazor (Primary/Error)
    public string BotonForeColor { get; set; } = "";

    // ── Reconocimiento facial ──
    public int  FaceTimeoutSegundos              { get; set; } = 30;    // segundos máximos para verificar identidad facial
    public bool UsarReconocimientoFacialEnBloqueo { get; set; } = false; // pedir reconocimiento facial al desbloquear pantalla

    // ── Configuración de Usuario (preferencias propias, no se serializan a XML general) ──
    [XmlIgnore] public int?     ZoomWebUsuario         { get; set; } // TAS_ZOOM_WEB; null = 100%
    [XmlIgnore] public int?     BloqueoMinutosUsuario  { get; set; } // NUM_MINUTOS_LOCKOUT_SCREEN; prevalece sobre BloqueoMinutos
    [XmlIgnore] public string?  FondoWorkspaceBase64   { get; set; } // IMG_FONDO_WORKSPACE en base64 (PNG)
    [XmlIgnore] public bool     EsZurdo                { get; set; } // FLG_USUARIO_SURDO; true = sidebar a la derecha
    [XmlIgnore] public bool     NotificacionesActivas  { get; set; } = true; // FLG_NOTIFICACION_ACTIVA; false = oculta el grupo de íconos del AppBar

    // Teclado virtual: se habilita solo si los 3 flags están en true (General AND Opción AND Usuario).
    public bool                 TecladoVirtual         { get; set; } // FLG_TECLADO_VIRTUAL de Configuración General (serializado)
    [XmlIgnore] public bool     TecladoVirtualUsuario  { get; set; } // FLG_TECLADO_VIRTUAL de Configuración de Usuario

    // ── Colores del menú contextual (clic derecho) ──
    public string MenuContextualFondo { get; set; } = "#2b2622";
    public string MenuContextualTexto { get; set; } = "#ffffff";

    // ── Colores del Login (independientes de los del entorno) ──
    public string LoginFondo { get; set; } = "#16130f";
    public string LoginTexto { get; set; } = "#efe9df";

    // ── Colores de los Diálogos (crear/editar/ver/eliminar) ──
    public string DialogFondo { get; set; } = "#efe9df";
    public string DialogTexto { get; set; } = "#16130f";

    // ── Color de los iconos de acciones en grillas (Editar/Ver/etc; Eliminar siempre rojo) ──
    public string IconoAccionesColor { get; set; } = "";
}
