using KONSolutions.Shared.Models;
using KONSolutions.Shared.Models.Seguridad;

namespace KONSolutions.Web.Services;

/// <summary>
/// Cachea, para el usuario logueado, si su perfil tiene FLG_SEGURIDAD = 1 Y FLG_AUDITORIA = 1.
/// Lo usan las pantallas CRUD para decidir si muestran el botón/panel de auditoría.
/// Ambos flags son requeridos: un perfil de Seguridad sin Auditoría no ve el botón,
/// y viceversa.
/// </summary>
public class SeguridadContextService
{
    private readonly IAuthService _auth;
    private readonly CatalogoService<Perfil, PerfilDto> _perfiles;
    private bool? _veAuditoria;   // null = aún no resuelto

    public SeguridadContextService(IAuthService auth, CatalogoService<Perfil, PerfilDto> perfiles)
    {
        _auth = auth;
        _perfiles = perfiles;
    }

    /// <summary>
    /// Devuelve true si el perfil del usuario logueado tiene FLG_SEGURIDAD = 1 Y FLG_AUDITORIA = 1.
    /// El resultado se cachea tras la primera consulta.
    /// </summary>
    public async Task<bool> EsPerfilSeguridadAsync()
    {
        if (_veAuditoria.HasValue) return _veAuditoria.Value;

        try
        {
            var sesion = await _auth.GetSesionAsync();
            if (sesion is null)
            {
                _veAuditoria = false;
                return false;
            }

            // 1) Si la sesión ya trae ambos flags (del SP de login), usarlos directo.
            if (sesion.FLG_SEGURIDAD && sesion.FLG_AUDITORIA)
            {
                _veAuditoria = true;
                return true;
            }

            // 2) Si la sesión no confirma ambos, consultar el perfil para verificar.
            //    (cubre el caso de sesiones antiguas guardadas antes de este campo).
            if (!string.IsNullOrWhiteSpace(sesion.COD_PERFIL_USUARIO))
            {
                var perfil = await _perfiles.GetByIdAsync(sesion.COD_PERFIL_USUARIO);
                _veAuditoria = (perfil?.FLG_SEGURIDAD ?? false) && (perfil?.FLG_AUDITORIA ?? false);
            }
            else
            {
                _veAuditoria = false;
            }
        }
        catch
        {
            _veAuditoria = false;
        }
        return _veAuditoria.Value;
    }

    /// <summary>Limpia el cache (al cerrar sesión).</summary>
    public void Reset() => _veAuditoria = null;
}
