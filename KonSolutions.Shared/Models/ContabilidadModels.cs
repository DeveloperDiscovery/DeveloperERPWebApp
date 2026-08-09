namespace KONSolutions.Shared.Models.Contabilidad;

/// <summary>Catálogo CONTABILIDAD.TIPOS_CENTRO_COSTO.</summary>
public class TipoCentroCosto
{
    public string COD_TIPO_CENTRO_COSTO { get; set; } = "";
    public string DES_TIPO_CENTRO_COSTO { get; set; } = "";
}

public class TipoCentroCostoDto
{
    public string COD_TIPO_CENTRO_COSTO { get; set; } = "";
    public string DES_TIPO_CENTRO_COSTO { get; set; } = "";
}

/// <summary>Catálogo jerárquico CONTABILIDAD.CENTROS_COSTO (auto-referenciada por COD_CENTRO_COSTO_PADRE).</summary>
public class CentroCosto
{
    public string  COD_CENTRO_COSTO       { get; set; } = "";
    public string  DES_CENTRO_COSTO       { get; set; } = "";
    public string  COD_TIPO_CENTRO_COSTO  { get; set; } = "";
    public int?    NUM_NIVEL              { get; set; }
    public string  COD_CENTRO_COSTO_PADRE { get; set; } = "";
    public bool?   FLG_GENERA_ASIENTO     { get; set; }
    public int?    COD_TIPO_ESTADO        { get; set; }
    public int?    COD_ESTADO             { get; set; }
    public string? DES_TIPO_CENTRO_COSTO  { get; set; }
    public string? DES_ESTADO             { get; set; }
    public string? DES_BACKCOLOR          { get; set; }
    public string? DES_FORECOLOR          { get; set; }
    public string  COD_USUARIO_REGISTRO       { get; set; } = "";
    public string  COD_ESTACION_REGISTRO      { get; set; } = "";
    public DateTime? FEC_REGISTRO             { get; set; }
    public string  COD_USUARIO_ACTUALIZACION  { get; set; } = "";
    public string  COD_ESTACION_ACTUALIZACION { get; set; } = "";
    public DateTime? FEC_ACTUALIZACION        { get; set; }

    /// <summary>Construido en el cliente al armar el árbol — no viene de la API.</summary>
    public List<CentroCosto> Hijos { get; set; } = new();
}

public class CentroCostoDto
{
    public string COD_CENTRO_COSTO       { get; set; } = "";
    public string DES_CENTRO_COSTO       { get; set; } = "";
    public string COD_TIPO_CENTRO_COSTO  { get; set; } = "";
    public int?   NUM_NIVEL              { get; set; }
    public string COD_CENTRO_COSTO_PADRE { get; set; } = "";
    public bool?  FLG_GENERA_ASIENTO     { get; set; }
    public int?   COD_TIPO_ESTADO        { get; set; }
    public int?   COD_ESTADO             { get; set; }
}
