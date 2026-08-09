using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Ingenieria;

/// <summary>
/// INGENIERIA.GRUPOS_FAMILIAS_SUB_FAMILIAS_GRUPOS_ESPECIFICACION_TECNICA — catálogo simple
/// de Grupos de Especificación Técnica (código + descripción), sin relación directa a
/// Grupo/Familia/Sub Familia de Producto. COD_GRUPO_ESPECIFICACION_TECNICA es IDENTITY —
/// se genera solo, no lo asigna el usuario.
/// </summary>
public class GruposEspecificacionTecnicaGrupo
{
    public int COD_GRUPO_ESPECIFICACION_TECNICA { get; set; }
    public string? DES_GRUPO_ESPECIFICACION_TECNICA { get; set; }
    public string COD_USUARIO_REGISTRO { get; set; } = "";
    public string COD_ESTACION_REGISTRO { get; set; } = "";
    public DateTime? FEC_REGISTRO { get; set; }
}

public class GruposEspecificacionTecnicaGrupoInsertDto
{
    public int COD_GRUPO_ESPECIFICACION_TECNICA { get; set; }
    public string? DES_GRUPO_ESPECIFICACION_TECNICA { get; set; }
}
