using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Comercial;

/// <summary>Catálogo COMERCIAL.TIPOS_PRECIO (COD identity).</summary>
public class TiposPrecio
{
    public int?    COD_TIPO_PRECIO { get; set; }
    [Required] public string DES_TIPO_PRECIO { get; set; } = string.Empty;
    public int?    COD_TIPO_ESTADO { get; set; }
    public int?    COD_ESTADO      { get; set; }
    public byte[]? IMG_ICONO       { get; set; }

    // Solo lectura (vienen del JOIN con ESTADOS en el SHOWALL/SEARCH).
    public string? DES_ESTADO      { get; set; }
    public string? DES_BACKCOLOR   { get; set; }
    public string? DES_FORECOLOR   { get; set; }
}
