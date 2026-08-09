using System.ComponentModel.DataAnnotations;
namespace KONSolutions.Shared.Models.Logistica;

/// <summary>LOGISTICA.RESERVAS_STOCK</summary>
public class ReservasStock
{
    public int? NUM_RESERVA_STOCK { get; set; }
    public string DES_RESERVA_STOCK { get; set; } = string.Empty;
    public DateTime? FEC_RESERVA_STOCK { get; set; }
    public int? NUM_REQUERIMIENTO_STOCK { get; set; }
    public string COD_TIPO_RESERVA { get; set; } = string.Empty;
    public string DES_OBSERVACION { get; set; } = string.Empty;
    public int? COD_TIPO_ESTADO { get; set; }
    public int? COD_ESTADO { get; set; }
    public string COD_USUARIO_REGISTRO { get; set; } = string.Empty;
    public string COD_ESTACION_REGISTRO { get; set; } = string.Empty;
    public DateTime? FEC_REGISTRO { get; set; }
    public string COD_USUARIO_ACTUALIZACION { get; set; } = string.Empty;
    public string COD_ESTACION_ACTUALIZACION { get; set; } = string.Empty;
    public DateTime? FEC_ACTUALIZACION { get; set; }

    // Vienen resueltas por los JOIN del procedimiento: solo se muestran.
    public string DES_TIPO_RESERVA { get; set; } = string.Empty;
    public string DES_REQUERIMIENTO_STOCK { get; set; } = string.Empty;
    public string DES_ESTADO { get; set; } = string.Empty;
    public string DES_BACKCOLOR { get; set; } = string.Empty;
    public string DES_FORECOLOR { get; set; } = string.Empty;
}
