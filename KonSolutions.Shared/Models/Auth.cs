using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Auth;

public class LoginRequest
{
    [Required(ErrorMessage = "El usuario es obligatorio")]
    public string COD_USUARIO { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    public string COD_PASSWORD { get; set; } = string.Empty;
}

public class LoginResponse
{
    public string  Token     { get; set; } = string.Empty;
    public string  TokenType { get; set; } = "Bearer";
    public int     ExpiresIn { get; set; }
    public UsuarioSesion Usuario { get; set; } = new();
}

public class UsuarioSesion
{
    public bool    FLG_ACCESO              { get; set; }
    public bool    FLG_CHANGE_PASSSWORD    { get; set; }   // 1 = debe cambiar password al ingresar
    public string  COD_USUARIO             { get; set; } = string.Empty;
    public string? DES_USUARIO             { get; set; }
    public string? COD_PERFIL_USUARIO      { get; set; }
    public string? DES_PERFIL_USUARIO      { get; set; }
    public string? COD_ROL_USUARIO         { get; set; }
    public string? DES_ROL_USUARIO         { get; set; }
    public string? DES_CORREO_INSTITUCIONAL{ get; set; }
    public string? NUM_TELEFONO            { get; set; }
    public bool    FLG_VALIDA_DIRECTORIO_ACTIVO { get; set; }
    public bool?   FLG_LOGIN_CREDENCIALES  { get; set; }   // Permite login con usuario+password
    public bool?   FLG_LOGIN_FACE          { get; set; }   // Requiere verificación facial tras credenciales
    public bool?   FLG_REGISTRO_FACIAL     { get; set; }   // Tiene biometría registrada en el servidor
    public bool?   FLG_LOGIN_HUELLA_DACTILAR { get; set; }
    public bool    FLG_SEGURIDAD           { get; set; }   // 1 = perfil de seguridad (ve panel auditoría)
    public bool    FLG_AUDITORIA           { get; set; }   // 1 = perfil con permiso de auditoría (junto a FLG_SEGURIDAD)
    public string? COD_ESTADO              { get; set; }
    public string? DES_ESTADO              { get; set; }
    /// <summary>Entidad (ESTANDAR.ENTIDADES) vinculada a este usuario y su entidad de
    /// dependencia, si aplica — nuevas columnas de PROC_SEGURIDAD_USUARIOS_VALIDALOGIN.
    /// Quedan guardadas en la sesión (localStorage "usuarioSesion") junto con el resto,
    /// recuperables vía AuthService.GetSesionAsync().</summary>
    public decimal? COD_ENTIDAD            { get; set; }
    public decimal? COD_ENTIDAD_DEPENDENCIA { get; set; }
    public string?  DES_ENTIDAD_DEPENDENCIA { get; set; }
    public byte[]?  IMG_LOGO_DEPENDENCIA    { get; set; }
}
