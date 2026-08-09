using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Ingenieria;

/// <summary>Catálogo INGENIERIA.DISENOS.</summary>
public class Disenos
{
    public string COD_DISENO { get; set; } = "";
    public string DES_DISENO { get; set; } = "";
    public string COD_TIPO_DISENO { get; set; } = "";
    public string COD_GRUPO_PRODUCTO { get; set; } = "";
    public string COD_FAMILIA_PRODUCTO { get; set; } = "";
    public string COD_SUB_FAMILIA_PRODUCTO { get; set; } = "";
    public string? COD_UNIDAD_MEDIDA { get; set; }
    public string? COD_UNIDAD_MEDIDA_SECUNDARIA { get; set; }
    public decimal? COD_ENTIDAD { get; set; }
    public int? NUM_TEMPORADA { get; set; }
    public int? NUM_COLECCION { get; set; }
    public int? NUM_SOLICITUD_COMERICAL { get; set; }
    public int? COD_TIPO_ESTADO { get; set; }
    public int? COD_ESTADO { get; set; }
    public string COD_USUARIO_REGISTRO { get; set; } = "";
    public string COD_ESTACION_REGISTRO { get; set; } = "";
    public DateTime? FEC_REGISTRO { get; set; }
    public string COD_USUARIO_ACTUALIZACION { get; set; } = "";
    public string COD_ESTACION_ACTUALIZACION { get; set; } = "";
    public DateTime? FEC_ACTUALIZACION { get; set; }

    // Solo lectura (JOINs).
    public string? DES_COMERCIAL { get; set; }
    public string? DES_MATERNO { get; set; }
    public string? DES_NOMBRE { get; set; }
    public string? DES_NOMBRE_COMPLETO { get; set; }
    public string? DES_NOMBRE2 { get; set; }
    public string? DES_PATERNO { get; set; }
    public string? DES_TIPO_DISENO { get; set; }
    public string? DES_SUB_FAMILIA_PRODUCTO { get; set; }
    public string? DES_BACKCOLOR { get; set; }
    public string? DES_ESTADO { get; set; }
    // Alias del estado (ESTANDAR.ESTADOS): 'ANU' deja el diseño en solo lectura.
    // Ver KONSolutions.Shared.Common.EstadoAlias.
    public string? COD_ESTADO_ALIAS { get; set; }
    public string? DES_FORECOLOR { get; set; }
    public string? DES_UNIDAD_MEDIDA { get; set; }
    public string? DES_TEMPORADA { get; set; }
    public string? DES_COLECCION { get; set; }
    public string? DES_GRUPO_PRODUCTO { get; set; }
    public string? DES_FAMILIA_PRODUCTO { get; set; }
    public string? DES_OBSERVACIONES { get; set; }
    public string? DES_SOLICITUD_COMERCIAL { get; set; }

    // Entidad responsable de la solicitud comercial vinculada. El SP la trae con
    // LEFT JOIN ESTANDAR.ENTIDADES j ON i.COD_ENTIDAD_RESPONSABLE = j.COD_ENTIDAD.
    public string? DES_ENTIDAD_ASIGNADA { get; set; }
}

/// <summary>DTO de guardado (PROC_INGENIERIA_DISENOS_INSERT es upsert — siempre POST).</summary>
public class DisenosDto
{
    // Sin [Required]: al crear un Diseño nuevo el código lo arma el SP (correlativo
    // vía FNC_ObtenerSiguienteCorrelativo) y este campo queda vacío en el formulario.
    public string COD_DISENO { get; set; } = "";
    [Required] public string DES_DISENO { get; set; } = "";
    [Required] public string COD_TIPO_DISENO { get; set; } = "";
    [Required] public string COD_GRUPO_PRODUCTO { get; set; } = "";
    [Required] public string COD_FAMILIA_PRODUCTO { get; set; } = "";
    [Required] public string COD_SUB_FAMILIA_PRODUCTO { get; set; } = "";
    public string? COD_UNIDAD_MEDIDA { get; set; }
    public string? COD_UNIDAD_MEDIDA_SECUNDARIA { get; set; }
    public decimal? COD_ENTIDAD { get; set; }
    public int? NUM_TEMPORADA { get; set; }
    public int? NUM_COLECCION { get; set; }
    // No es obligatorio.
    public int? NUM_SOLICITUD_COMERICAL { get; set; }
    public int? COD_TIPO_ESTADO { get; set; }
    public int? COD_ESTADO { get; set; }
}

public class DisenosSearchDto
{
    public string? Search { get; set; }
}

/// <summary>Respuesta de /ingenieria/disenos/loadcombos — un solo roundtrip para los combos
/// del diálogo (Tipos de Diseño, Grupos de Producto, Unidades de Medida, Entidades cliente).</summary>
public class DisenosLoadCombosResult
{
    public int CodTipoEstado { get; set; }
    public List<KONSolutions.Shared.Common.ComboboxItem> TiposDiseno { get; set; } = new();
    public List<KONSolutions.Shared.Common.ComboboxItem> GruposProducto { get; set; } = new();
    public List<KONSolutions.Shared.Common.ComboboxItem> UnidadesMedida { get; set; } = new();
    public List<KONSolutions.Shared.Common.ComboboxItem> Clientes { get; set; } = new();
    public List<KONSolutions.Shared.Common.EstadoItem> Estados { get; set; } = new();
}
