using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Ingenieria;

// ───────────── INGENIERIA.DISENOS_VERSION ─────────────
// Clave compuesta COD_DISENO + NUM_VERSION. Todos los SP de listado filtran por
// COD_DISENO: una versión solo existe dentro de su diseño.
// Ojo: COD_RUTA_PRODUCCION es nvarchar(100) en esta tabla, no el int de PRODUCCION_RUTAS.
public class DisenoVersion
{
    [Required] public string COD_DISENO { get; set; } = "";
    public int? NUM_VERSION { get; set; }
    public string? DES_VERSION { get; set; }
    public string? COD_CLASE_DESARROLLO { get; set; }
    public string? COD_TIPO_DESARROLLO { get; set; }
    public string? COD_RUTA_PRODUCCION { get; set; }
    public string? DES_OBSERVACIONES { get; set; }
    public int? COD_TIPO_ESTADO { get; set; }
    public int? COD_ESTADO { get; set; }
    public string COD_USUARIO_REGISTRO { get; set; } = "";
    public string COD_ESTACION_REGISTRO { get; set; } = "";
    public DateTime? FEC_REGISTRO { get; set; }
    public string COD_USUARIO_ACTUALIZACION { get; set; } = "";
    public string COD_ESTACION_ACTUALIZACION { get; set; } = "";
    public DateTime? FEC_ACTUALIZACION { get; set; }

    // Solo lectura (JOINs de los SP).
    public string? DES_DISENO { get; set; }
    public string? DES_BACKCOLOR { get; set; }
    public string? DES_ESTADO { get; set; }
    // Alias del estado (ESTANDAR.ESTADOS): 'ANU' deja la versión en solo lectura.
    // Ver KONSolutions.Shared.Common.EstadoAlias.
    public string? COD_ESTADO_ALIAS { get; set; }
    public string? DES_FORECOLOR { get; set; }
    public string? DES_CLASE_DESARROLLO { get; set; }
    public string? DES_TIPO_DESARROLLO { get; set; }
    public string? DES_RUTA_PRODUCCION { get; set; }
}

/// <summary>Respuesta de /ingenieria/disenos-version/loadcombos — un solo roundtrip para
/// los combos del diálogo de versión (Clase de Desarrollo y Ruta de Producción).</summary>
public class DisenosVersionLoadCombosResult
{
    public int CodTipoEstado { get; set; }
    public List<KONSolutions.Shared.Common.ComboboxItem> ClasesDesarrollo { get; set; } = new();
    public List<KONSolutions.Shared.Common.ComboboxItem> ProduccionRutas { get; set; } = new();
    public List<KONSolutions.Shared.Common.ComboboxItem> UnidadesMedida { get; set; } = new();
    public List<KONSolutions.Shared.Common.EstadoItem> Estados { get; set; } = new();
}
