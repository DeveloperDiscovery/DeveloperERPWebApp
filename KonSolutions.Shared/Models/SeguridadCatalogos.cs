using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Seguridad;

// ───────────── PERFILES_USUARIO ─────────────
public class Perfil
{
    public string  COD_PERFIL_USUARIO       { get; set; } = string.Empty;
    public string? DES_PERFIL_USUARIO       { get; set; }
    public string? COD_PASSWORD_COMPLEJIDAD { get; set; }
    public string? DES_PASSWORD_COMPLEJIDAD { get; set; } // Descripción de la complejidad (para la grilla)
    public bool    FLG_DEVELOPER            { get; set; }
    public bool    FLG_ADMINISTRADOR        { get; set; }
    public bool    FLG_SEGURIDAD            { get; set; }
    public bool    FLG_AUDITORIA            { get; set; }
    public bool    FLG_COMERCIAL            { get; set; }
    public bool    FLG_DEPENDENCIA_COMERCIAL { get; set; }
    public int?    COD_TIPO_ESTADO          { get; set; }
    public int?    COD_ESTADO               { get; set; }
    public string? DES_ESTADO               { get; set; }
    public string? COD_USUARIO_REGISTRO       { get; set; }
    public string? COD_ESTACION_REGISTRO      { get; set; }
    public DateTime? FEC_REGISTRO              { get; set; }
    public string? COD_USUARIO_ACTUALIZACION  { get; set; }
    public string? COD_ESTACION_ACTUALIZACION { get; set; }
    public DateTime? FEC_ACTUALIZACION         { get; set; }
    public byte[]? IMG_ICONO                  { get; set; }
}

public class PerfilDto
{
    [Required] public string COD_PERFIL_USUARIO { get; set; } = string.Empty;
    [Required] public string DES_PERFIL_USUARIO { get; set; } = string.Empty;
    public string? COD_PASSWORD_COMPLEJIDAD { get; set; }
    public bool FLG_DEVELOPER     { get; set; }
    public bool FLG_ADMINISTRADOR { get; set; }
    public bool FLG_SEGURIDAD     { get; set; }
    public bool FLG_AUDITORIA     { get; set; }
    public bool FLG_COMERCIAL     { get; set; }
    public bool FLG_DEPENDENCIA_COMERCIAL { get; set; }
    public int  COD_TIPO_ESTADO   { get; set; }
    public int?  COD_ESTADO        { get; set; } = 1;
    public byte[]? IMG_ICONO       { get; set; }
}

// ───────────── ROLES_USUARIO ─────────────
public class Rol
{
    public string  COD_ROL_USUARIO { get; set; } = string.Empty;
    public string? DES_ROL_USUARIO { get; set; }
    public int?    COD_TIPO_ESTADO { get; set; }
    public int?    COD_ESTADO      { get; set; }
    public string? DES_ESTADO      { get; set; }
    public string? COD_USUARIO_REGISTRO       { get; set; }
    public string? COD_ESTACION_REGISTRO      { get; set; }
    public DateTime? FEC_REGISTRO             { get; set; }
    public string? COD_USUARIO_ACTUALIZACION  { get; set; }
    public string? COD_ESTACION_ACTUALIZACION { get; set; }
    public DateTime? FEC_ACTUALIZACION        { get; set; }
}

public class RolDto
{
    [Required] public string COD_ROL_USUARIO { get; set; } = string.Empty;
    [Required] public string DES_ROL_USUARIO { get; set; } = string.Empty;
    public int COD_TIPO_ESTADO { get; set; }
    public int? COD_ESTADO       { get; set; } = 1;
}

// ───────────── COMPLEJIDAD_PASSWORD ─────────────
public class ComplejidadPwd
{
    public string  COD_PASSWORD_COMPLEJIDAD          { get; set; } = string.Empty;
    public string? DES_PASSWORD_COMPLEJIDAD          { get; set; }
    public string? DES_CARACTERES_LETRAS_MAYUSCULAS  { get; set; }
    public string? DES_CARACTERES_LETRAS_MINUSCULAS  { get; set; }
    public string? DES_CARACTERES_NUMERICOS          { get; set; }
    public string? DES_CARACTERES_ESPECIALES         { get; set; }
    public int?    CAN_CARACTERES_MINIMO             { get; set; }
    public int?    CAN_CARACTERES_MAXIMO             { get; set; }
    public int?    NUM_ORDEN_PRESENTACION            { get; set; }
    public int?    COD_TIPO_ESTADO                   { get; set; }
    public int?    COD_ESTADO                        { get; set; }
    public string? DES_ESTADO                        { get; set; }
    public string? COD_USUARIO_REGISTRO       { get; set; }
    public string? COD_ESTACION_REGISTRO      { get; set; }
    public DateTime? FEC_REGISTRO             { get; set; }
    public string? COD_USUARIO_ACTUALIZACION  { get; set; }
    public string? COD_ESTACION_ACTUALIZACION { get; set; }
    public DateTime? FEC_ACTUALIZACION        { get; set; }
}

public class ComplejidadPwdDto
{
    [Required] public string COD_PASSWORD_COMPLEJIDAD { get; set; } = string.Empty;
    [Required] public string DES_PASSWORD_COMPLEJIDAD { get; set; } = string.Empty;
    public string? DES_CARACTERES_LETRAS_MAYUSCULAS { get; set; }
    public string? DES_CARACTERES_LETRAS_MINUSCULAS { get; set; }
    public string? DES_CARACTERES_NUMERICOS         { get; set; }
    public string? DES_CARACTERES_ESPECIALES        { get; set; }
    [Range(1, 100)] public int CAN_CARACTERES_MINIMO { get; set; } = 8;
    [Range(1, 100)] public int CAN_CARACTERES_MAXIMO { get; set; } = 20;
    public int NUM_ORDEN_PRESENTACION { get; set; }
    public int COD_TIPO_ESTADO { get; set; }
    public int? COD_ESTADO       { get; set; } = 1;
}

// ───────────── APLICACIONES ─────────────
public class Aplicacion
{
    public string  COD_APLICACION  { get; set; } = string.Empty;
    public string? DES_APLICACION  { get; set; }
    public bool    FLG_APP_MOVIL   { get; set; }
    public byte[]? IMG_ICONO       { get; set; }
    public string  DES_BACKCOLOR   { get; set; } = "";
    public string  DES_FORECOLOR   { get; set; } = "";
    public int?    COD_TIPO_ESTADO { get; set; }
    public int?    COD_ESTADO      { get; set; }
    public string? DES_ESTADO      { get; set; }
    public string? DES_BACKCOLOR_ESTADO { get; set; }
    public string? DES_FORECOLOR_ESTADO { get; set; }
    public string? COD_USUARIO_REGISTRO       { get; set; }
    public string? COD_ESTACION_REGISTRO      { get; set; }
    public DateTime? FEC_REGISTRO             { get; set; }
    public string? COD_USUARIO_ACTUALIZACION  { get; set; }
    public string? COD_ESTACION_ACTUALIZACION { get; set; }
    public DateTime? FEC_ACTUALIZACION        { get; set; }
}

public class AplicacionDto
{
    [Required] public string COD_APLICACION { get; set; } = string.Empty;
    [Required] public string DES_APLICACION { get; set; } = string.Empty;
    public bool    FLG_APP_MOVIL   { get; set; }
    public byte[]? IMG_ICONO       { get; set; }
    public string  DES_BACKCOLOR   { get; set; } = "";
    public string  DES_FORECOLOR   { get; set; } = "";
    public int  COD_TIPO_ESTADO { get; set; }
    public int?  COD_ESTADO      { get; set; } = 1;
}

// ───────────── SEGURIDAD.BOTONES_ESTANDAR ─────────────
public class BotonEstandar
{
    public string? COD_BOTON_OPCION_ESTANDAR { get; set; }
    public string? DES_BOTON_OPCION_ESTANDAR { get; set; }
    public bool?   FLG_GRILLA                { get; set; }
    public bool?   FLG_HEADER                { get; set; }
    // Verbos HTTP que este botón habilita por defecto (heredados por cada botón
    // de opción que lo referencie vía COD_BOTON_OPCION_ESTANDAR).
    public bool?   FLG_ACCION_POST           { get; set; }
    public bool?   FLG_ACCION_PUT            { get; set; }
    public bool?   FLG_ACCION_DELETE         { get; set; }
    public bool?   FLG_ACCION_GET            { get; set; }
    // Método de la API a invocar dinámicamente (ej. "Insert", "Delete").
    public string? DES_METODO                { get; set; }
    public byte[]? IMG_ICONO { get; set; }
    public string? COD_USUARIO_REGISTRO       { get; set; }
    public string? COD_ESTACION_REGISTRO      { get; set; }
    public DateTime? FEC_REGISTRO             { get; set; }
    public string? COD_USUARIO_ACTUALIZACION  { get; set; }
    public string? COD_ESTACION_ACTUALIZACION { get; set; }
    public DateTime? FEC_ACTUALIZACION        { get; set; }
}

public class BotonEstandarDto
{
    [Required] public string COD_BOTON_OPCION_ESTANDAR { get; set; } = string.Empty;
    [Required] public string DES_BOTON_OPCION_ESTANDAR { get; set; } = string.Empty;
    public bool? FLG_GRILLA        { get; set; }
    public bool? FLG_HEADER        { get; set; }
    public bool? FLG_ACCION_POST   { get; set; }
    public bool? FLG_ACCION_PUT    { get; set; }
    public bool? FLG_ACCION_DELETE { get; set; }
    public bool? FLG_ACCION_GET    { get; set; }
    public string? DES_METODO      { get; set; }
    public byte[]? IMG_ICONO { get; set; }
}

// ───────────── OPCIONES APLICACIONES TREEVIEW ─────────────
public class NodoOpcion
{
    public string  COD_PARENT              { get; set; } = "";
    public string  COD_CHILDREN            { get; set; } = "";
    public string  DES_OPCION              { get; set; } = "";
    public string  COD_OPCION              { get; set; } = "";
    public string  COD_APLICACION_ORIGINAL { get; set; } = "";
    public string  COD_OPCION_ORIGINAL     { get; set; } = "";
    public string  COD_BOTON_ORIGINAL      { get; set; } = "";
    public string  DES_PAGINA             { get; set; } = "";
    public string  COD_TIPO               { get; set; } = ""; // ROT, APP, CRP, MDL, BTN
    public byte[]? IMG_ICONO              { get; set; }

    public int?    COD_ESTADO                  { get; set; }
    // Descripciones del SP actualizado
    public string  DES_APLICACION              { get; set; } = "";
    public string  DES_OPCION_APLICACION        { get; set; } = "";
    public string  DES_OPCION_APLICACION_PADRE  { get; set; } = "";
    public string  COD_OPCION_APLICACION_PADRE  { get; set; } = "";
    public bool?   FLG_EXIGE_PASSWORD           { get; set; }
    public bool?   FLG_TECLADO_VIRTUAL          { get; set; }

    /// <summary>Nodos hijos — se arma en memoria, no viene del SP.</summary>
    public List<NodoOpcion> Hijos { get; set; } = new();

    /// <summary>Base64 de IMG_ICONO para mostrar en Blazor.</summary>
    public string? IconoBase64 => IMG_ICONO is { Length: > 0 }
        ? Convert.ToBase64String(IMG_ICONO) : null;
}

/// <summary>Fila de GET /seguridad/aplicaciones-opciones-treeview/logos — experimento: íconos
/// de TODO el árbol en un endpoint aparte del texto principal (Showall), identificados por el
/// mismo par COD_PARENT/COD_CHILDREN que arma la jerarquía en el cliente.</summary>
public class NodoOpcionLogo
{
    public string  COD_PARENT   { get; set; } = "";
    public string  COD_CHILDREN { get; set; } = "";
    public byte[]? IMG_ICONO    { get; set; }
}

/// <summary>Fila plana de GET /seguridad/aplicaciones-opciones — usada para fusionar
/// FLG_EXIGE_PASSWORD/FLG_TECLADO_VIRTUAL en el árbol de OpcionesAplicaciones.razor
/// sin depender de que el SP del treeview traiga esas columnas.</summary>
public class AplicacionesOpcionesFlagsRow
{
    public string COD_APLICACION        { get; set; } = "";
    public string COD_OPCION_APLICACION { get; set; } = "";
    public bool?  FLG_EXIGE_PASSWORD    { get; set; }
    public bool?  FLG_TECLADO_VIRTUAL   { get; set; }
}

// ───────────── APLICACIONES OPCIONES BOTONES ─────────────
public class AplicacionesOpcionesBotones
{
    public string  COD_APLICACION              { get; set; } = "";
    public string  COD_OPCION_APLICACION       { get; set; } = "";
    public string  COD_BOTON_OPCION            { get; set; } = "";
    public string  DES_BOTON_OPCION            { get; set; } = "";
    public string  DES_TOOLTIPTEXT             { get; set; } = "";
    public string  DES_ALIAS                   { get; set; } = "";
    public string  COD_BOTON_OPCION_ESTANDAR   { get; set; } = "";
    public byte[]? IMG_ICONO                   { get; set; }
    public bool    FLG_GRILLA                  { get; set; }
    public bool    FLG_HEADER                  { get; set; }
    // Verbos HTTP que este botón habilita — base de la autorización por rol+botón.
    public bool?   FLG_ACCION_POST             { get; set; }
    public bool?   FLG_ACCION_PUT              { get; set; }
    public bool?   FLG_ACCION_DELETE           { get; set; }
    public bool?   FLG_ACCION_GET              { get; set; }
    public string? DES_METODO                  { get; set; }
    public int?    COD_TIPO_ESTADO             { get; set; }
    public int?    COD_ESTADO                  { get; set; }
    public string  DES_FORMULARIO              { get; set; } = "";
    public string  DES_OPCION_APLICACION       { get; set; } = "";
    public string  DES_BOTON_OPCION_ESTANDAR   { get; set; } = "";
    public string  DES_BACKCOLOR               { get; set; } = "";
    public string  DES_ESTADO                  { get; set; } = "";
    public string  DES_FORECOLOR               { get; set; } = "";
    public string? COD_USUARIO_REGISTRO        { get; set; }
    public string? COD_ESTACION_REGISTRO       { get; set; }
    public DateTime? FEC_REGISTRO              { get; set; }
    public string? COD_USUARIO_ACTUALIZACION   { get; set; }
    public string? COD_ESTACION_ACTUALIZACION  { get; set; }
    public DateTime? FEC_ACTUALIZACION         { get; set; }
}

public class AplicacionesOpcionesBotonesInsertDto
{
    public string  COD_APLICACION              { get; set; } = "";
    public string  COD_OPCION_APLICACION       { get; set; } = "";
    public string  COD_BOTON_OPCION            { get; set; } = "";
    public string  DES_BOTON_OPCION            { get; set; } = "";
    public string? DES_TOOLTIPTEXT             { get; set; }
    public string? DES_ALIAS                   { get; set; }
    // Null en el insert; el backend conserva el valor existente en el update.
    public string? COD_BOTON_OPCION_ESTANDAR   { get; set; }
    public byte[]? IMG_ICONO                   { get; set; }
    public bool    FLG_GRILLA                  { get; set; }
    public bool    FLG_HEADER                  { get; set; }
    public bool?   FLG_ACCION_POST             { get; set; }
    public bool?   FLG_ACCION_PUT              { get; set; }
    public bool?   FLG_ACCION_DELETE           { get; set; }
    public bool?   FLG_ACCION_GET              { get; set; }
    public string? DES_METODO                  { get; set; }
    public int?    COD_TIPO_ESTADO             { get; set; }
    public int?    COD_ESTADO                  { get; set; }
}

// ───────────── TIPOS_OPCIONES_APLICACION ─────────────
public class TipoOpcion
{
    public string  COD_TIPO_OPCION_APLICACION { get; set; } = string.Empty;
    public string? DES_TIPO_OPCION_APLICACION { get; set; }
    public bool?   FLG_MODULO                { get; set; }
    public bool?   FLG_SEPARADOR             { get; set; }
    public bool?   FLG_CARPETA               { get; set; }
    public byte[]? IMG_ICONO                 { get; set; }
}

public class AplicacionesOpcionesFormDto
{
    [Required] public string COD_APLICACION              { get; set; } = string.Empty;
               public string COD_OPCION_APLICACION       { get; set; } = string.Empty;
               public string COD_OPCION_APLICACION_PADRE { get; set; } = string.Empty;
    [Required] public string DES_OPCION_APLICACION       { get; set; } = string.Empty;
    [Required] public string COD_TIPO_OPCION_APLICACION  { get; set; } = string.Empty;
               public string DES_FORMULARIO              { get; set; } = string.Empty;
               // Parámetro fijo de la opción (ej. "CLI") — sobrevive a mover la opción de
               // carpeta, ver DES_PARAMETRO en SEGURIDAD.APLICACIONES_OPCIONES.
               public string? DES_PARAMETRO              { get; set; }
               // Tabla de negocio que administra la opción. La usa el guardián de permisos de
               // la API: si no viaja en el guardado, editar la opción la deja en NULL y el rol
               // pierde el permiso sin aviso.
               public string? DES_OBJETO                { get; set; }
               public bool   FLG_EXIGE_PASSWORD          { get; set; } = false;
               public bool   FLG_TECLADO_VIRTUAL         { get; set; } = false;
               public byte[]? IMG_ICONO                  { get; set; }
               public string  DES_BACKCOLOR              { get; set; } = "";
               public string  DES_FORECOLOR              { get; set; } = "";
               public int?   COD_TIPO_ESTADO             { get; set; }
               public int?   COD_ESTADO                  { get; set; } = 1;
               public string  COD_USUARIO_REGISTRO       { get; set; } = "";
               public string  COD_ESTACION_REGISTRO      { get; set; } = "";
               public DateTime? FEC_REGISTRO             { get; set; }
               public string  COD_USUARIO_ACTUALIZACION  { get; set; } = "";
               public string  COD_ESTACION_ACTUALIZACION { get; set; } = "";
               public DateTime? FEC_ACTUALIZACION        { get; set; }
}

public class TipoOpcionDto
{
    [Required] public string COD_TIPO_OPCION_APLICACION { get; set; } = string.Empty;
    [Required] public string DES_TIPO_OPCION_APLICACION { get; set; } = string.Empty;
    public bool? FLG_MODULO    { get; set; }
    public bool? FLG_SEPARADOR { get; set; }
    public bool? FLG_CARPETA   { get; set; }
    public byte[]? IMG_ICONO   { get; set; }
}
