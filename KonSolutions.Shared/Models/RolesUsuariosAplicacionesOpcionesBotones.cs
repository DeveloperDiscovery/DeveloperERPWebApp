namespace KONSolutions.Shared.Models.Seguridad;

/// <summary>Fila de SEGURIDAD.ROLES_USUARIOS_APLICACIONES_OPCIONES_BOTONES, con los
/// datos del botón (join) — usada para habilitar/deshabilitar botones por rol.</summary>
public class RolBotonOpcion
{
    public string COD_ROL_USUARIO { get; set; } = string.Empty;
    public string COD_APLICACION { get; set; } = string.Empty;
    public string COD_OPCION_APLICACION { get; set; } = string.Empty;
    public string COD_BOTON_OPCION { get; set; } = string.Empty;
    public bool? FLG_HABILITADO { get; set; }
    public string? DES_BOTON_OPCION { get; set; }
    public string? DES_ALIAS { get; set; }
    public string? DES_TOOLTIPTEXT { get; set; }
    public bool? FLG_HEADER { get; set; }
    public bool? FLG_GRILLA { get; set; }
}
