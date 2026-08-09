using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Estandar;

// ───────────── ESTANDAR.TIPOS_USO ─────────────
// Tabla mínima: código nvarchar(3) + descripción nvarchar(25). No lleva estado
// ni campos de auditoría, por eso el diálogo no muestra EstadoCombo ni PanelAuditoria.
public class TiposUso
{
    [Required] public string COD_TIPO_USO { get; set; } = "";
    [Required] public string DES_TIPO_USO { get; set; } = "";
}
