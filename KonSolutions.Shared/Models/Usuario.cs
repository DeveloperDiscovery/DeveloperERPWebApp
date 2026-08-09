using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Seguridad;

/// <summary>
/// Entidad USUARIOS (schema SEGURIDAD). Campos exactos del SHOWALL/SEARCH/SHOWBYID.
/// Incluye los campos nuevos de control de password (días para reset, fecha del
/// último cambio) e imágenes (foto y firma del usuario).
/// </summary>
public class Usuario
{
    public string  COD_USUARIO                  { get; set; } = string.Empty;
    public string? DES_USUARIO                  { get; set; }
    public string? COD_PERFIL_USUARIO           { get; set; }
    public string? DES_PERFIL_USUARIO           { get; set; }
    public string? COD_ROL_USUARIO              { get; set; }
    public string? DES_ROL_USUARIO              { get; set; }
    public string? DES_CORREO_INSTITUCIONAL     { get; set; }
    public string? NUM_TELEFONO                 { get; set; }
    public bool    FLG_VALIDA_DIRECTORIO_ACTIVO { get; set; }
    public bool?   FLG_LOGIN_CREDENCIALES       { get; set; }
    public bool?   FLG_LOGIN_FACE               { get; set; }
    public bool?   FLG_LOGIN_HUELLA_DACTILAR    { get; set; }
    public bool?   FLG_REGISTRO_FACIAL          { get; set; }
    public int?    NUM_MAXIMO_INTENTOS          { get; set; }
    public int?    NUM_INTENTOS                 { get; set; }
    public int?    NUM_DIAS_PASSWORD_RESET      { get; set; }   // Días antes de exigir reset
    public DateTime? FEC_ULTIMA_PASSWORD_CHANGE { get; set; }   // Fecha del último cambio de password
    public byte[]? IMG_FOTO                     { get; set; }   // Foto del usuario
    public byte[]? IMG_FIRMA                    { get; set; }   // Firma del usuario
    public int?    COD_TIPO_ESTADO              { get; set; }
    public int?    COD_ESTADO                   { get; set; }
    public string? COD_USUARIO_REGISTRO        { get; set; }
    public string? COD_ESTACION_REGISTRO       { get; set; }
    public DateTime? FEC_REGISTRO               { get; set; }
    public string? COD_USUARIO_ACTUALIZACION   { get; set; }
    public string? COD_ESTACION_ACTUALIZACION  { get; set; }
    public DateTime? FEC_ACTUALIZACION          { get; set; }
    public string? DES_ESTADO                   { get; set; }
    public string? DES_BACKCOLOR                { get; set; }
    public string? DES_FORECOLOR                { get; set; }
}

/// <summary>
/// DTO de alta de usuario (PROC_SEGURIDAD_USUARIOS_INSERT).
/// El password NO se captura aquí: va en NULL al crear; el usuario lo define
/// en su primer login (flujo FLG_CHANGE_PASSSWORD). Incluye foto y firma.
/// </summary>
public class UsuarioInsertDto
{
    [Required(ErrorMessage = "El código es obligatorio")]
    public string COD_USUARIO { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string DES_USUARIO { get; set; } = string.Empty;

    [Required(ErrorMessage = "El perfil es obligatorio")]
    public string COD_PERFIL_USUARIO { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Correo inválido")]
    public string? DES_CORREO_INSTITUCIONAL { get; set; }

    public string? NUM_TELEFONO { get; set; }

    public bool  FLG_VALIDA_DIRECTORIO_ACTIVO { get; set; }
    public bool? FLG_LOGIN_CREDENCIALES       { get; set; }
    public bool? FLG_LOGIN_FACE               { get; set; }
    public bool? FLG_LOGIN_HUELLA_DACTILAR    { get; set; }
    public bool? FLG_REGISTRO_FACIAL          { get; set; }

    [Range(1, 100)] public int NUM_MAXIMO_INTENTOS { get; set; } = 3;
    public int NUM_INTENTOS { get; set; } = 0;
    public int NUM_DIAS_PASSWORD_RESET { get; set; } = 90;   // Por defecto 90 días
    public byte[]? IMG_FOTO  { get; set; }                   // Foto del usuario
    public byte[]? IMG_FIRMA { get; set; }                   // Firma del usuario

    /// <summary>Entidad (ESTANDAR.ENTIDADES) elegida desde el picker de "Usuario Comercial"
    /// o "Usuario de Dependencia Comercial" — el SP INSERT la vincula a este usuario.</summary>
    public decimal? COD_ENTIDAD { get; set; }

    public int COD_TIPO_ESTADO { get; set; }
    public int COD_ESTADO      { get; set; } = 1;
}

/// <summary>
/// DTO de modificación (mismos campos; el SP hace upsert por COD_USUARIO).
/// Tampoco captura password (se gestiona por el flujo de cambio/reset).
/// </summary>
public class UsuarioUpdateDto
{
    [Required] public string COD_USUARIO { get; set; } = string.Empty;
    [Required] public string DES_USUARIO { get; set; } = string.Empty;
    [Required] public string COD_PERFIL_USUARIO { get; set; } = string.Empty;
    [EmailAddress] public string? DES_CORREO_INSTITUCIONAL { get; set; }
    public string? NUM_TELEFONO { get; set; }
    public bool  FLG_VALIDA_DIRECTORIO_ACTIVO { get; set; }
    public bool? FLG_LOGIN_CREDENCIALES       { get; set; }
    public bool? FLG_LOGIN_FACE               { get; set; }
    public bool? FLG_LOGIN_HUELLA_DACTILAR    { get; set; }
    [Range(1, 100)] public int NUM_MAXIMO_INTENTOS { get; set; } = 3;
    public int NUM_INTENTOS { get; set; }
    public int NUM_DIAS_PASSWORD_RESET { get; set; } = 90;
    public byte[]? IMG_FOTO  { get; set; }
    public byte[]? IMG_FIRMA { get; set; }
    public int COD_TIPO_ESTADO { get; set; }
    public int COD_ESTADO      { get; set; }
}

/// <summary>
/// Cambio de contraseña (PROC_SEGURIDAD_USUARIOS_PASSWORD).
/// Se usa tanto en el cambio voluntario como en el cambio forzado del login.
/// </summary>
public class CambiarPasswordRequest
{
    [Required] public string COD_USUARIO  { get; set; } = string.Empty;
    [Required] public string COD_PASSWORD { get; set; } = string.Empty;
    // Rol ACTIVO del usuario que hace la petición — necesario para que la API
    // verifique si tiene el botón "Cambiar Password" habilitado y así poder
    // cambiar la contraseña de OTRO usuario (no solo la propia).
    public string? COD_ROL_USUARIO_ACTIVO { get; set; }
}

/// <summary>
/// Reset de contraseña (PROC_SEGURIDAD_USUARIOS_PASSWORDRESET).
/// Deja el password en NULL para forzar su redefinición en el próximo login.
/// </summary>
public class ResetPasswordRequest
{
    [Required] public string COD_USUARIO { get; set; } = string.Empty;
}

/// <summary>Cambio de estado (PROC_SEGURIDAD_USUARIOS_ESTADOS).</summary>
public class CambiarEstadoRequest
{
    [Required] public string COD_USUARIO { get; set; } = string.Empty;
    [Required] public int    COD_ESTADO  { get; set; }
}
