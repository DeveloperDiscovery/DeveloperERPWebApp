using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Comercial;

/// <summary>Catálogo COMERCIAL.TIPOS_SOLICITUD_COMERCIAL.</summary>
public class TiposSolicitudComercial
{
    public string COD_TIPO_SOLICITUD_COMERICAL { get; set; } = "";
    public string? DES_TIPO_SOLICITUD_COMERICAL { get; set; }
}

/// <summary>DTO de guardado (PROC_COMERCIAL_TIPOS_SOLICITUD_COMERCIAL_INSERT es upsert — siempre POST).</summary>
public class TiposSolicitudComercialDto
{
    [Required] public string COD_TIPO_SOLICITUD_COMERICAL { get; set; } = "";
    [Required] public string DES_TIPO_SOLICITUD_COMERICAL { get; set; } = "";
}

/// <summary>Sub-módulo hijo: áreas administrativas asignadas a un tipo de solicitud
/// comercial (tabla puente COMERCIAL.TIPOS_SOLICITUD_COMERCIAL_AREAS).</summary>
public class TiposSolicitudComercialAreas
{
    public string COD_TIPO_SOLICITUD_COMERICAL { get; set; } = "";
    public int?   COD_AREA { get; set; }
    public string COD_USUARIO_REGISTRO { get; set; } = "";
    public string COD_ESTACION_REGISTRO { get; set; } = "";
    public DateTime? FEC_REGISTRO { get; set; }
    public string? DES_TIPO_SOLICITUD_COMERICAL { get; set; }
    public string? DES_AREA { get; set; }
}

/// <summary>DTO de alta (PROC_COMERCIAL_TIPOS_SOLICITUD_COMERCIAL_AREAS_INSERT es upsert — siempre POST).</summary>
public class TiposSolicitudComercialAreasDto
{
    [Required] public string COD_TIPO_SOLICITUD_COMERICAL { get; set; } = "";
    [Required] public int?   COD_AREA { get; set; }
}

/// <summary>Sub-módulo nieto: cargos de cada área participantes del flujo de aprobación
/// de un tipo de solicitud comercial (tabla puente TIPOS_SOLICITUD_COMERCIAL_AREAS_CARGOS).</summary>
public class TiposSolicitudComercialAreasCargos
{
    public string COD_TIPO_SOLICITUD_COMERICAL { get; set; } = "";
    public int?   COD_AREA { get; set; }
    public int?   COD_CARGO { get; set; }
    public int?   NUM_NIVEL_FLUJO { get; set; }
    public bool?  FLG_APROBADOR { get; set; }
    public bool?  FLG_NOTIFICADOR { get; set; }
    public string COD_USUARIO_REGISTRO { get; set; } = "";
    public string COD_ESTACION_REGISTRO { get; set; } = "";
    public DateTime? FEC_REGISTRO { get; set; }
    public string? DES_TIPO_SOLICITUD_COMERICAL { get; set; }
    public string? DES_AREA { get; set; }
    public string? DES_CARGO { get; set; }
}

/// <summary>DTO de alta (PROC_..._AREAS_CARGOS_INSERT es upsert — siempre POST).</summary>
public class TiposSolicitudComercialAreasCargosDto
{
    [Required] public string COD_TIPO_SOLICITUD_COMERICAL { get; set; } = "";
    [Required] public int?   COD_AREA { get; set; }
    [Required] public int?   COD_CARGO { get; set; }
    public int?   NUM_NIVEL_FLUJO { get; set; }
    public bool?  FLG_APROBADOR { get; set; }
    public bool?  FLG_NOTIFICADOR { get; set; }
}
