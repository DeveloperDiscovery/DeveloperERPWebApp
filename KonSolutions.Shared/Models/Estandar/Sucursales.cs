using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Estandar;

public class Sucursales
{
    [Required] public string COD_SUCURSAL { get; set; } = string.Empty;
    [Required] public string DES_SUCURSAL { get; set; } = string.Empty;
    [Required] public string DES_DIRECCION { get; set; } = string.Empty;
    [Required] public string COD_UBIGEO { get; set; } = string.Empty;
    [Required] public string COD_TIPO_SUCURSAL { get; set; } = string.Empty;
    public decimal? COD_ENTIDAD_PROPIETARIA { get; set; }
    public string? DES_LINK_PATH_FACTURACION { get; set; }
    public string? DES_TOKEN_FACTURACION { get; set; }
    public string? COD_TIPO_POS { get; set; }
    public int? COD_TIPO_DIRECCION { get; set; }
    public decimal? VAL_LATITUD { get; set; }
    public decimal? VAL_LONGITUD { get; set; }
    public int? COD_TIPO_ESTADO { get; set; }
    public int? COD_ESTADO { get; set; }
    public string? COD_USUARIO_REGISTRO { get; set; }
    public string? COD_ESTACION_REGISTRO { get; set; }
    public DateTime? FEC_REGISTRO { get; set; }
    public string? COD_USUARIO_ACTUALIZACION { get; set; }
    public string? COD_ESTACION_ACTUALIZACION { get; set; }
    public DateTime? FEC_ACTUALIZACION { get; set; }

    // Descripciones (join) para mostrar en grilla/dialog, no se envían al guardar
    public string? DES_UBIGEO { get; set; }
    public string? DES_TIPO_POS { get; set; }
    public string? DES_TIPO_DIRECCION { get; set; }
    public string? DES_ESTADO { get; set; }
    public string? DES_BACKCOLOR { get; set; }
    public string? DES_FORECOLOR { get; set; }
    public string? DES_NOMBRE_COMPLETO { get; set; }
}

/// <summary>Resultado de /estandar/sucursales/loadcombos — trae Ubigeo (ya
/// filtrado a distritos), Entidades activas y Tipos de POS en un solo viaje.</summary>
public class SucursalesLoadCombosResult
{
    public List<KONSolutions.Shared.Common.ComboboxItem> Ubigeos        { get; set; } = new();
    public List<KONSolutions.Shared.Common.ComboboxItem> Entidades      { get; set; } = new();
    public List<KONSolutions.Shared.Common.ComboboxItem> TiposPos       { get; set; } = new();
    public List<KONSolutions.Shared.Common.ComboboxItem> TiposDireccion { get; set; } = new();
}
