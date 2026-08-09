using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Estandar;

public class TiposSucursales
{
    [Required] public string COD_TIPO_SUCURSAL { get; set; } = string.Empty;
    [Required] public string DES_TIPO_SUCURSAL { get; set; } = string.Empty;
    public bool? FLG_SERVICIO { get; set; }
    public bool? FLG_PLANTA { get; set; }
    public bool? FLG_ALMACEN { get; set; }
    public bool? FLG_ADMINISTRACION { get; set; }
    public bool? FLG_TIENDA { get; set; }
    public bool? FLG_CALLCENTER { get; set; }
    public bool? FLG_DESARROLLO { get; set; }
    public bool? FLG_EXPLOTACION { get; set; }
    public bool? FLG_PUERTO { get; set; }
    public bool? FLG_AEROPUERTO { get; set; }
    public bool? FLG_TERMINAL_TERRESTRE { get; set; }
    public bool? FLG_PATIO_MANIOBRAS { get; set; }
    public bool? FLG_CENTRO_MEDICO { get; set; }
    public bool? FLG_HOTEL { get; set; }
    public bool? FLG_CENTRO_DATOS { get; set; }
    public bool? FLG_CENTRO_DISTRIBUCION { get; set; }
    public bool? FLG_MONITOREO { get; set; }
    public bool? FLG_FUNDO { get; set; }
    public bool? FLG_CAMPAMENTO { get; set; }
    public bool? FLG_ADUANA { get; set; }
}
