namespace KONSolutions.Shared.Models.Seguridad;

/// <summary>
/// Opción de menú autorizada para el rol.
/// SP PROC_SEGURIDAD_ROLES_USUARIOS_APLICACIONES_OPCIONES_SHOWALL
/// </summary>
public class OpcionMenu
{
    public string  COD_ROL_USUARIO        { get; set; } = string.Empty;
    public string  COD_APLICACION         { get; set; } = string.Empty;
    public string? DES_APLICACION         { get; set; }
    public string  COD_OPCION_APLICACION  { get; set; } = string.Empty;
    public bool    FLG_ACCESO_PERMITIDO   { get; set; }
    public string? DES_FORMULARIO         { get; set; }
    public string? DES_OPCION_APLICACION  { get; set; }
    public string? DES_ROL_USUARIO        { get; set; }
    public string? IMG_ICONO_OPCION       { get; set; }  // base64, ícono propio de la opción en la BD (si existe)
    public bool?   FLG_EXIGE_PASSWORD     { get; set; }
}

/// <summary>
/// Botón autorizado dentro de una opción.
/// SP PROC_SEGURIDAD_ROLES_USUARIOS_APLICACIONES_OPCIONES_BOTONES_SEARCH
/// </summary>
public class BotonOpcion
{
    public string  COD_ROL_USUARIO       { get; set; } = string.Empty;
    public int     COD_APLICACION        { get; set; }
    public int     COD_OPCION_APLICACION { get; set; }
    public int     COD_BOTON_OPCION      { get; set; }
    public bool    FLG_HABILITADO        { get; set; }
    public bool    FLG_ACCESO_PERMITIDO  { get; set; }
    public string? DES_ALIAS             { get; set; }
    public string? DES_BOTON_OPCION      { get; set; }
    public string? DES_TOOLTIPTEXT       { get; set; }
}

/// <summary>Favorito del usuario (persistido localmente por ahora).</summary>
public class Favorito
{
    public string  COD_OPCION_APLICACION { get; set; } = string.Empty;
    public string  Titulo  { get; set; } = string.Empty;
    public string  Ruta    { get; set; } = string.Empty;
    public string? Icono   { get; set; }
}

public class FavoritoWorkspace
{
    public string  COD_APLICACION        { get; set; } = string.Empty;
    public string? DES_APLICACION        { get; set; }
    public string  COD_OPCION_APLICACION { get; set; } = string.Empty;
    public string? DES_OPCION_APLICACION { get; set; }
    public string? DES_FORMULARIO        { get; set; }
    public byte[]? IMG_ICONO_OPCION      { get; set; }
    public byte[]? IMG_ICONO_APLICACION  { get; set; }
    public string  DES_BACKCOLOR_APLICACION { get; set; } = "";
    public string  DES_FORECOLOR_APLICACION { get; set; } = "";
    public string  DES_BACKCOLOR_OPCION     { get; set; } = "";
    public string  DES_FORECOLOR_OPCION     { get; set; } = "";
}

/// <summary>Fila de GET /seguridad/favoritos/workspace/logos/{COD_USUARIO} — experimento:
/// íconos de los favoritos del Workspace en un endpoint aparte del texto (GetWorkspace),
/// identificados por el mismo par COD_APLICACION/COD_OPCION_APLICACION.</summary>
public class FavoritoLogoItem
{
    public string  COD_APLICACION        { get; set; } = string.Empty;
    public string  COD_OPCION_APLICACION { get; set; } = string.Empty;
    public byte[]? IMG_ICONO_OPCION      { get; set; }
    public byte[]? IMG_ICONO_APLICACION  { get; set; }
}

// ───── Vistas de asignaciones del usuario (tabs en la pantalla de Usuarios) ─────
public class RolAsignado
{
    public string? COD_ROL_USUARIO { get; set; }
    public string? DES_ROL_USUARIO { get; set; }
    public string? COD_USUARIO     { get; set; }
    public string? DES_USUARIO     { get; set; }
}

public class OpcionAsignada
{
    public string? COD_ROL_USUARIO       { get; set; }
    public string? COD_APLICACION        { get; set; }
    public string? COD_OPCION_APLICACION { get; set; }
    public bool    FLG_ACCESO_PERMITIDO  { get; set; }
    public string? DES_FORMULARIO        { get; set; }
    public string? DES_OPCION_APLICACION { get; set; }
}

public class BotonAsignado
{
    public string? COD_ROL_USUARIO       { get; set; }
    public string? COD_APLICACION        { get; set; }
    public string? COD_OPCION_APLICACION { get; set; }
    public string? COD_BOTON_OPCION      { get; set; }
    public bool    FLG_HABILITADO        { get; set; }
    public string? DES_ALIAS             { get; set; }
    public string? DES_BOTON_OPCION      { get; set; }
    public byte[]? IMG_ICONO             { get; set; }
    public string? IconoBase64 => IMG_ICONO is { Length: > 0 }
        ? Convert.ToBase64String(IMG_ICONO) : null;
}

/// <summary>
/// Item del SP PROC_SEGURIDAD_ROLES_USUARIOS_APLICACIONES_OPCIONES_BOTONES_TREEVIEW.
/// Trae, en una sola llamada, TODOS los botones de TODAS las opciones del rol activo —
/// se carga una vez al login (o al cambiar de rol) y se filtra en memoria por pantalla,
/// sin volver a llamar a la API en cada navegación.
/// </summary>
public class BotonTreeviewItem
{
    public string  COD_ROL_USUARIO        { get; set; } = string.Empty;
    public string  COD_APLICACION         { get; set; } = string.Empty;
    public string? DES_APLICACION         { get; set; }
    // Tipo de la opción (módulo, carpeta, formulario, etc.) — usado para distinguir
    // nodos de navegación de pantallas reales en el árbol del menú.
    public string? COD_TIPO_OPCION_APLICACION { get; set; }
    public string? DES_TIPO_OPCION_APLICACION { get; set; }
    // Código de la opción contenedora (carpeta/módulo) de este nodo. Null o vacío
    // si es un nodo raíz, sin padre — usado para construir el árbol jerárquico.
    public string? COD_OPCION_APLICACION_PADRE { get; set; }
    public string  COD_OPCION_APLICACION  { get; set; } = string.Empty;
    public string? DES_OPCION_APLICACION  { get; set; }
    public string? DES_FORMULARIO         { get; set; }
    // Nullable: con el LEFT JOIN hacia botones, una opción sin ningún botón
    // configurado aún aparece en el resultado (con estas columnas en NULL).
    public string? COD_BOTON_OPCION       { get; set; }
    public string? DES_BOTON_OPCION       { get; set; }
    public string? DES_TOOLTIPTEXT        { get; set; }
    public string? DES_ALIAS              { get; set; }
    public bool?   FLG_HABILITADO         { get; set; }
    public bool?   FLG_HEADER             { get; set; }
    public bool?   FLG_GRILLA             { get; set; }
    public bool?   FLG_ACCION_PUT         { get; set; }
    public bool?   FLG_ACCION_POST        { get; set; }
    public bool?   FLG_ACCION_DELETE      { get; set; }
    public bool?   FLG_ACCION_GET         { get; set; }
    // Nombre del método del Controller de la API a invocar dinámicamente
    // (ej. "Insert", "Delete", "GetById") — sin schema/tabla por delante; la
    // tabla se resuelve cruzando con DES_FORMULARIO de la opción.
    public string? DES_METODO             { get; set; }
    public string  DES_BACKCOLOR_APLICACION { get; set; } = "";
    public string  DES_FORECOLOR_APLICACION { get; set; } = "";
    public string? IMG_ICONO_APLICACION     { get; set; }  // base64
    public string  DES_BACKCOLOR_OPCION     { get; set; } = "";
    public string  DES_FORECOLOR_OPCION     { get; set; } = "";
    public string? IMG_ICONO_OPCION         { get; set; }  // base64
    public bool?   FLG_EXIGE_PASSWORD       { get; set; }
    // Ícono propio del botón (experimento, cargado aparte en segundo plano — ver
    // MenuService.CargarLogosEnSegundoPlanoAsync). null hasta que llegue o si el SP
    // trae NULL para este botón — AccionesBotones.razor cae al ícono calculado.
    public string? IMG_ICONO_BOTON          { get; set; }  // base64
}

// ───── Resultado deduplicado del TREEVIEW (3 result sets, ver MenuService.cs) ─────

/// <summary>Resultado 1/3: una fila por aplicación, ícono UNA sola vez.</summary>
public class AplicacionTreeviewItem
{
    public string  COD_APLICACION           { get; set; } = string.Empty;
    public string  DES_APLICACION           { get; set; } = string.Empty;
    public string  DES_BACKCOLOR_APLICACION { get; set; } = "";
    public string  DES_FORECOLOR_APLICACION { get; set; } = "";
    public string? IMG_ICONO_APLICACION     { get; set; }  // base64
}

/// <summary>Resultado 2/3: una fila por opción, ícono UNA sola vez.</summary>
public class OpcionTreeviewItem
{
    public string  COD_APLICACION              { get; set; } = string.Empty;
    public string? COD_TIPO_OPCION_APLICACION  { get; set; }
    public string? DES_TIPO_OPCION_APLICACION  { get; set; }
    public string? COD_OPCION_APLICACION_PADRE { get; set; }
    public string  COD_OPCION_APLICACION       { get; set; } = string.Empty;
    public string  DES_OPCION_APLICACION       { get; set; } = string.Empty;
    public string  DES_FORMULARIO              { get; set; } = string.Empty;
    public string  DES_BACKCOLOR_OPCION        { get; set; } = "";
    public string  DES_FORECOLOR_OPCION        { get; set; } = "";
    public string? IMG_ICONO_OPCION            { get; set; }  // base64
    public bool?   FLG_TECLADO_VIRTUAL         { get; set; }  // habilita teclado virtual en esta opción
    public bool?   FLG_EXIGE_PASSWORD         { get; set; }  // exige contraseña al abrir esta opción
}

/// <summary>Resultado 3/3: un botón por fila (sin colores ni íconos).</summary>
public class BotonTreeviewItemLite
{
    public string  COD_APLICACION              { get; set; } = string.Empty;
    public string? COD_OPCION_APLICACION_PADRE { get; set; }
    public string  COD_OPCION_APLICACION       { get; set; } = string.Empty;
    public string? COD_BOTON_OPCION            { get; set; }
    public string? DES_BOTON_OPCION            { get; set; }
    public string? DES_TOOLTIPTEXT             { get; set; }
    public string? DES_ALIAS                   { get; set; }
    public bool?   FLG_HABILITADO              { get; set; }
    public bool?   FLG_HEADER                  { get; set; }
    public bool?   FLG_GRILLA                  { get; set; }
    public bool?   FLG_ACCION_PUT              { get; set; }
    public bool?   FLG_ACCION_POST             { get; set; }
    public bool?   FLG_ACCION_DELETE           { get; set; }
    public bool?   FLG_ACCION_GET              { get; set; }
    public string? DES_METODO                  { get; set; }
}

/// <summary>Envoltorio de los 3 resultados — esto es lo que se cachea en localStorage
/// (compacto, sin íconos repetidos) en vez del List&lt;BotonTreeviewItem&gt; aplanado.</summary>
public class TreeviewResult
{
    public List<AplicacionTreeviewItem> Aplicaciones { get; set; } = new();
    public List<OpcionTreeviewItem>     Opciones     { get; set; } = new();
    public List<BotonTreeviewItemLite>  Botones      { get; set; } = new();
}

// ───── Resultado de GET .../treeview/logos (experimento: íconos aparte del texto) ─────

/// <summary>1/3: ícono de cada aplicación del rol.</summary>
public class AplicacionLogoItem
{
    public string  COD_APLICACION       { get; set; } = string.Empty;
    public string? IMG_ICONO_APLICACION { get; set; }  // base64
}

/// <summary>2/3: ícono de cada opción del rol.</summary>
public class OpcionLogoItem
{
    public string  COD_APLICACION              { get; set; } = string.Empty;
    public string? COD_OPCION_APLICACION_PADRE { get; set; }
    public string  COD_OPCION_APLICACION       { get; set; } = string.Empty;
    public string? IMG_ICONO_OPCION            { get; set; }  // base64
}

/// <summary>3/3: ícono propio de un botón específico — cuando venga NULL, el frontend
/// cae al ícono calculado por nombre/flags (ver AccionesBotones.IconoEstandar).</summary>
public class BotonLogoItem
{
    public string  COD_APLICACION        { get; set; } = string.Empty;
    public string  COD_OPCION_APLICACION { get; set; } = string.Empty;
    public string? COD_BOTON_OPCION      { get; set; }
    public string? IMG_ICONO             { get; set; }  // base64
}

/// <summary>Envoltorio de los 3 resultados del TREEVIEWLOGOS.</summary>
public class TreeviewLogosResult
{
    public List<AplicacionLogoItem> Aplicaciones { get; set; } = new();
    public List<OpcionLogoItem>     Opciones     { get; set; } = new();
    public List<BotonLogoItem>      Botones      { get; set; } = new();
}

/// <summary>
/// Rol asignado al usuario logueado (un usuario puede tener varios).
/// SP PROC_SEGURIDAD_ROLES_USUARIO_USUARIOS_SHOWBYUSUARIO.
/// </summary>
public class RolDeUsuario
{
    public string  COD_ROL_USUARIO { get; set; } = string.Empty;
    public string? DES_ROL_USUARIO { get; set; }
}

/// <summary>
/// Nodo del árbol jerárquico del menú lateral, construido a partir de
/// BotonTreeviewItem agrupando por COD_OPCION_APLICACION_PADRE.
///
/// Comportamiento por TipoOpcion (SEGURIDAD.TIPOS_OPCIONES):
/// - MDL (Módulo)     → opción real, clicable, abre su DES_FORMULARIO (igual
///                      que BTN). Si tiene hijos, también se muestran debajo.
/// - SMD (Sub Módulo) → no se muestra, pero sus hijos sí (se "suben" un nivel).
/// - CRP (Carpeta)    → expandible/colapsable, contiene hijos.
/// - BTN (Botón)      → opción real, abre su pantalla al hacer clic.
/// - SPR/ROT (Separador) → visual decorativo, sin hijos clicables.
/// </summary>
public class NodoMenu
{
    public string  COD_OPCION_APLICACION       { get; set; } = string.Empty;
    public string  COD_APLICACION              { get; set; } = string.Empty;
    public string? DES_APLICACION              { get; set; }
    public string? DES_OPCION_APLICACION       { get; set; }
    public string? DES_FORMULARIO              { get; set; }
    public string? COD_TIPO_OPCION_APLICACION  { get; set; }
    public string  DES_BACKCOLOR_APLICACION    { get; set; } = "";
    public string  DES_FORECOLOR_APLICACION    { get; set; } = "";
    public string? IMG_ICONO_APLICACION        { get; set; }
    public string  DES_BACKCOLOR_OPCION        { get; set; } = "";
    public string  DES_FORECOLOR_OPCION        { get; set; } = "";
    public string? IMG_ICONO_OPCION            { get; set; }
    public bool?   FLG_EXIGE_PASSWORD          { get; set; }
    public List<NodoMenu> Hijos                { get; set; } = new();

    public bool EsModulo    => (COD_TIPO_OPCION_APLICACION ?? "").Trim() == "MDL";
    public bool EsSubModulo => (COD_TIPO_OPCION_APLICACION ?? "").Trim() == "SMD";
    public bool EsCarpeta   => (COD_TIPO_OPCION_APLICACION ?? "").Trim() == "CRP";
    public bool EsBoton     => (COD_TIPO_OPCION_APLICACION ?? "").Trim() == "BTN";
    public bool EsSeparador => (COD_TIPO_OPCION_APLICACION ?? "").Trim() is "SPR" or "ROT";
}
