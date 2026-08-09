using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Comercial;

public class TiposMedioContacto
{
    public int?    COD_TIPO_MEDIO_CONTACTO { get; set; }
    [Required] public string DES_TIPO_MEDIO_CONTACTO { get; set; } = string.Empty;
    public bool?   FLG_CORREO    { get; set; }
    public bool?   FLG_TELEFONO  { get; set; }
    public bool?   FLG_WEB       { get; set; }
    public bool?   FLG_RED_SOCIAL { get; set; }
    public byte[]? IMG_ICONO     { get; set; }
}
