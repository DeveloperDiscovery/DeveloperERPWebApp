namespace KONSolutions.Shared.Models.Estandar;

/// <summary>Fila sugerida para asignar como usuario del sistema, a partir de una
/// ESTANDAR.ENTIDADES sin usuario relacionado todavía. Alimenta el picker que se abre
/// desde el ícono junto a Perfil en UsuarioDialog.razor (Usuario Comercial / Usuario de
/// Dependencia Comercial) — DES_ENTIDAD_DEPENDENCIA solo viene en el segundo caso.</summary>
public class EntidadUsuarioSugerido
{
    public decimal? COD_ENTIDAD { get; set; }
    public string DES_ENTIDAD { get; set; } = "";
    public string? DES_ENTIDAD_DEPENDENCIA { get; set; }
    public int? COD_TIPO_DOCUMENTO_IDENTIDAD { get; set; }
    public string NUM_DOCUMENTO_IDENTIDAD { get; set; } = "";
    public string? DES_PREFIJO { get; set; }
    public string COD_USUARIO_SUGERIDO { get; set; } = "";
}
