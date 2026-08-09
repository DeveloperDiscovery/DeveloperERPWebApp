using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Controlcalidad;

// ───────────── CONTROLCALIDAD.CLASES_DEFECTOS_TECNICOS ─────────────
public class ClasesDefectosTecnicos
{
    [Required] public string COD_CLASE_DEFECTO_TECNICO { get; set; } = "";
    [Required] public string DES_CLASE_DEFECTO_TECNICO { get; set; } = "";
}

// ───────────── CONTROLCALIDAD.TIPOS_DEFECTOS_TECNICOS ─────────────
public class TiposDefectosTecnicos
{
    [Required] public string COD_CLASE_DEFECTO_TECNICO { get; set; } = "";
    [Required] public string COD_TIPO_DEFECTO_TECNICO  { get; set; } = "";
    [Required] public string DES_DEFECTO_TECNICO       { get; set; } = "";
    public bool? FLG_OBSERVACION     { get; set; }
    public int?  COD_TIPO_ESTADO     { get; set; }
    public int?  COD_ESTADO          { get; set; }
    public string DES_ESTADO         { get; set; } = "";
    public string DES_BACKCOLOR      { get; set; } = "";
    public string DES_FORECOLOR      { get; set; } = "";
    public string DES_CLASE_DEFECTO_TECNICO { get; set; } = "";
    public string COD_USUARIO_REGISTRO       { get; set; } = "";
    public string COD_ESTACION_REGISTRO      { get; set; } = "";
    public DateTime? FEC_REGISTRO            { get; set; }
    public string COD_USUARIO_ACTUALIZACION  { get; set; } = "";
    public string COD_ESTACION_ACTUALIZACION { get; set; } = "";
    public DateTime? FEC_ACTUALIZACION       { get; set; }
}
