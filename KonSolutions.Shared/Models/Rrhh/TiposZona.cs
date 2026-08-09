using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Rrhh;

public class TiposZona
{
    [Required] public string COD_TIPO_ZONA { get; set; } = "";
    [Required] public string DES_TIPO_ZONA { get; set; } = "";
}
