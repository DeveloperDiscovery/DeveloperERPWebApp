namespace KONSolutions.Shared.Models.Entorno;

// ───────────── ENTORNO.DOMINIOS_EMPRESA ─────────────
// Dominios de correo válidos de la empresa (ej. "empresa.com"), usados al completar el
// correo institucional de un usuario. Código autonumérico — no lleva estado ni auditoría.
public class DominiosEmpresa
{
    public byte COD_DOMINIO_EMPRESA { get; set; }
    public string DES_DOMINIO_EMPRESA { get; set; } = "";
}
