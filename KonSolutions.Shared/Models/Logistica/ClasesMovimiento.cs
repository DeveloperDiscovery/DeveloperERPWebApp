using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Logistica;

/// <summary>LOGISTICA.CLASES_MOVIMIENTO — Padre de TiposMovimiento.
/// PROC_LOGISTICA_CLASES_MOVIMIENTO_INSERT es upsert — siempre POST.</summary>
public class ClasesMovimiento
{
    public string COD_CLASE_MOVIMIENTO { get; set; } = "";
    public string DES_CLASE_MOVIMIENTO { get; set; } = "";
    /// <summary>La clase aplica a movimientos de ALMACÉN.</summary>
    public bool? FLG_ALMACENES { get; set; }
    /// <summary>La clase aplica a movimientos de PRODUCCIÓN. No son excluyentes: una misma
    /// clase puede servir a los dos ámbitos, y ninguno de los dos es obligatorio.</summary>
    public bool? FLG_PRODUCCION { get; set; }
    public int? COD_TIPO_ESTADO { get; set; }
    public int? COD_ESTADO { get; set; }
    public string? DES_BACKCOLOR { get; set; }
    public string? DES_ESTADO { get; set; }
    public string? DES_FORECOLOR { get; set; }
}

public class ClasesMovimientoInsertDto
{
    [Required] public string COD_CLASE_MOVIMIENTO { get; set; } = "";
    [Required] public string DES_CLASE_MOVIMIENTO { get; set; } = "";
    public bool? FLG_ALMACENES { get; set; }
    public bool? FLG_PRODUCCION { get; set; }
    public int? COD_TIPO_ESTADO { get; set; }
    public int? COD_ESTADO { get; set; }
}
