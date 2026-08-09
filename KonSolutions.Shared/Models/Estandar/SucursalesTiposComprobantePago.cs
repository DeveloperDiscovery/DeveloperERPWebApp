using System.ComponentModel.DataAnnotations;
namespace KONSolutions.Shared.Models.Estandar;

/// <summary>ESTANDAR.SUCURSALES_TIPOS_COMPROBANTE_PAGO — serie y último correlativo de cada
/// tipo de comprobante habilitado en una sucursal. Hija de ESTANDAR.SUCURSALES.
/// PK compuesta: COD_SUCURSAL + COD_TIPO_COMPROBANTE_PAGO.</summary>
public class SucursalesTiposComprobantePago
{
    [Required] public string COD_SUCURSAL { get; set; } = "";
    [Required] public string COD_TIPO_COMPROBANTE_PAGO { get; set; } = "";
    public short? NUM_SERIE_COMPROBANTE { get; set; }
    public int? NUM_CORRELATIVO_ULTIMO { get; set; }

    // Solo lectura (JOINs).
    public string? DES_TIPO_COMPROBANTE_PAGO { get; set; }
    public string? DES_SUCURSAL { get; set; }
}
