using System.ComponentModel.DataAnnotations;
namespace KONSolutions.Shared.Models.Logistica;

/// <summary>LOGISTICA.REQUERIMIENTOS_STOCK</summary>
public class RequerimientosStock
{
    public int? NUM_REQUERIMIENTO_STOCK { get; set; }
    public string DES_REQUERIMIENTO_STOCK { get; set; } = string.Empty;
    public DateTime? FEC_REQUERIMIENTO_STOCK { get; set; }
    public string COD_TIPO_REQUERIMIENTO { get; set; } = string.Empty;
    public string COD_CENTRO_COSTO_REQUIRIENTE { get; set; } = string.Empty;
    public decimal? COD_ENTIDAD_REQUIRIENTE { get; set; }
    public string COD_CENTRO_COSTO_REQUIRIENTE_ORIGINAL { get; set; } = string.Empty;
    public decimal? COD_ENTIDAD_REQUIRIENTE_ORIGINAL { get; set; }
    public DateTime? FEC_ATENCION { get; set; }
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
    public string DES_TIPO_REQUERIMIENTO { get; set; } = string.Empty;
    public string DES_CENTRO_COSTO { get; set; } = string.Empty;
    public string DES_NOMBRE_COMPLETO { get; set; } = string.Empty;
    public string DES_COMERCIAL { get; set; } = string.Empty;
    public string DES_PATERNO { get; set; } = string.Empty;
    public string DES_MATERNO { get; set; } = string.Empty;
    public string DES_NOMBRE { get; set; } = string.Empty;
    public string DES_NOMBRE2 { get; set; } = string.Empty;
    public string DES_PREFIJO_USUARIO_COMERCIAL { get; set; } = string.Empty;
    public string DES_ESTADO { get; set; } = string.Empty;
    public string DES_BACKCOLOR { get; set; } = string.Empty;
    public string DES_FORECOLOR { get; set; } = string.Empty;
}
