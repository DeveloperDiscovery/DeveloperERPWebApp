namespace KONSolutions.Shared.Models.Entorno;

public class Servidor
{
    public string COD_KEY_SERVIDOR { get; set; } = string.Empty;
    public string COD_IP { get; set; } = string.Empty;
    public string DES_KEY_IP { get; set; } = string.Empty;
    public int? COD_TIPO_ESTADO { get; set; }
    public int? COD_ESTADO { get; set; }
    public string DES_BACKCOLOR { get; set; } = string.Empty;
    public string DES_ESTADO { get; set; } = string.Empty;
    public string DES_FORECOLOR { get; set; } = string.Empty;
    public string COD_USUARIO_REGISTRO       { get; set; } = string.Empty;
    public string COD_ESTACION_REGISTRO      { get; set; } = string.Empty;
    public DateTime? FEC_REGISTRO            { get; set; }
    public string COD_USUARIO_ACTUALIZACION  { get; set; } = string.Empty;
    public string COD_ESTACION_ACTUALIZACION { get; set; } = string.Empty;
    public DateTime? FEC_ACTUALIZACION       { get; set; }
}
