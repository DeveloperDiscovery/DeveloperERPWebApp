using System.ComponentModel.DataAnnotations;
namespace KONSolutions.Shared.Models.Ingenieria;

/// <summary>INGENIERIA.DISENOS_MEMORIAS — bitácora de anotaciones libres por diseño
/// ("Ayuda Memoria"). PK: COD_DISENO + NUM_SECUENCIA.</summary>
public class DisenosMemorias
{
    public string COD_DISENO { get; set; } = "";
    public short NUM_SECUENCIA { get; set; }
    public string? DES_MEMORIA { get; set; }
    public string? COD_USUARIO_REGISTRO { get; set; }
    public string? COD_ESTACION_REGISTRO { get; set; }
    public DateTime? FEC_REGISTRO { get; set; }
    public string? COD_USUARIO_ACTUALIZACION { get; set; }
    public string? COD_ESTACION_ACTUALIZACION { get; set; }
    public DateTime? FEC_ACTUALIZACION { get; set; }
}

public class DisenosMemoriasInsertDto
{
    [Required] public string COD_DISENO { get; set; } = "";
    [Required] public short NUM_SECUENCIA { get; set; }
    [Required] public string DES_MEMORIA { get; set; } = "";
}

/// <summary>INGENIERIA.DISENOS_VERSIONES_MEMORIAS — igual pero acotada a una versión.
/// PK: COD_DISENO + NUM_VERSION + NUM_SECUENCIA.</summary>
public class DisenosVersionesMemorias
{
    public string COD_DISENO { get; set; } = "";
    public short NUM_VERSION { get; set; }
    public short NUM_SECUENCIA { get; set; }
    public string? DES_MEMORIA { get; set; }
    public string? COD_USUARIO_REGISTRO { get; set; }
    public string? COD_ESTACION_REGISTRO { get; set; }
    public DateTime? FEC_REGISTRO { get; set; }
    public string? COD_USUARIO_ACTUALIZACION { get; set; }
    public string? COD_ESTACION_ACTUALIZACION { get; set; }
    public DateTime? FEC_ACTUALIZACION { get; set; }
}

public class DisenosVersionesMemoriasInsertDto
{
    [Required] public string COD_DISENO { get; set; } = "";
    [Required] public short NUM_VERSION { get; set; }
    [Required] public short NUM_SECUENCIA { get; set; }
    [Required] public string DES_MEMORIA { get; set; } = "";
}
