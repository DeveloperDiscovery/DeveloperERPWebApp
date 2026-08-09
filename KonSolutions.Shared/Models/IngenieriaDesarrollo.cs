using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Ingenieria;

// ───────────── INGENIERIA.CLASES_DESARROLLO ─────────────
// Tabla mínima: código nvarchar(3) + descripción nvarchar(50). No lleva estado
// ni campos de auditoría, por eso el diálogo no muestra EstadoCombo ni PanelAuditoria.
public class ClaseDesarrollo
{
    [Required] public string COD_CLASE_DESARROLLO { get; set; } = "";
    [Required] public string DES_CLASE_DESARROLLO { get; set; } = "";
}

// ───────────── INGENIERIA.TIPOS_DESARROLLO ─────────────
// Clave compuesta: la clase de desarrollo + el código del tipo.
public class TipoDesarrollo
{
    [Required] public string COD_CLASE_DESARROLLO { get; set; } = "";
    [Required] public string COD_TIPO_DESARROLLO  { get; set; } = "";
    [Required] public string DES_TIPO_DESARROLLO  { get; set; } = "";

    /// <summary>Solo lectura: la trae el LEFT JOIN de los SP contra CLASES_DESARROLLO.</summary>
    public string DES_CLASE_DESARROLLO { get; set; } = "";
}
