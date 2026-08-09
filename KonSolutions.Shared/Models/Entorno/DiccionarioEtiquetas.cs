using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Entorno;

/// <summary>ENTORNO.DICCIONARIO_ETIQUETAS — COD_ETIQUETA lo ingresa el usuario (no es
/// identity). PROC_ENTORNO_DICCIONARIO_ETIQUETAS_INSERT es upsert — siempre se llama POST.</summary>
public class DiccionarioEtiqueta
{
    public string COD_ETIQUETA { get; set; } = "";
    [Required] public string DES_ETIQUETA { get; set; } = "";
}
