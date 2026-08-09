namespace KONSolutions.Shared.Models.Biometria;

public class BiometriaEstadoDto
{
    public string COD_USUARIO   { get; set; } = string.Empty;
    public string DES_USUARIO   { get; set; } = string.Empty;
    public string? DES_ESTADO   { get; set; }
    public bool   FLG_REGISTRADO { get; set; }
    public int    NUM_MUESTRAS  { get; set; }
    public DateTime? FEC_REGISTRO { get; set; }
}

public class BiometriaResultadoDto
{
    public bool   Verificado    { get; set; }
    public double Distancia     { get; set; }
    public string Mensaje       { get; set; } = string.Empty;
}

public class BiometriaRegistrarRequest
{
    public string        COD_USUARIO  { get; set; } = string.Empty;
    public List<double[]> Descriptores { get; set; } = new();
}

public class BiometriaVerificarRequest
{
    public string   COD_USUARIO  { get; set; } = string.Empty;
    public double[] Descriptor   { get; set; } = Array.Empty<double>();
}
