using System.ComponentModel.DataAnnotations;

namespace KONSolutions.Shared.Models.Externo;

// ───────────── EXTERNO.SNIPPETS ─────────────
public class Snippet
{
    public int?    COD_SNIPPET    { get; set; }
    public string? DES_SNIPPET    { get; set; }
    public string? URL_SNIPPET    { get; set; }
    public string? DES_TOKEN      { get; set; }
    public bool    FLG_FREE_PAY   { get; set; }
    public string? COD_APLICACION         { get; set; }
    public string? DES_APLICACION         { get; set; }
    public string? COD_OPCION_APLICACION  { get; set; }
    public string? DES_OPCION_APLICACION  { get; set; }
    public string? COD_BOTON_OPCION       { get; set; }
    public string? DES_BOTON_OPCION       { get; set; }
    public int?    COD_TIPO_ESTADO { get; set; }
    public int?    COD_ESTADO      { get; set; }
    public string? DES_ESTADO      { get; set; }
    public string? DES_BACKCOLOR   { get; set; }
    public string? DES_FORECOLOR   { get; set; }
    public string? COD_USUARIO_REGISTRO       { get; set; }
    public string? COD_ESTACION_REGISTRO      { get; set; }
    public DateTime? FEC_REGISTRO             { get; set; }
    public string? COD_USUARIO_ACTUALIZACION  { get; set; }
    public string? COD_ESTACION_ACTUALIZACION { get; set; }
    public DateTime? FEC_ACTUALIZACION        { get; set; }
}

public class SnippetDto
{
    public int?   COD_SNIPPET     { get; set; }
    [Required] public string DES_SNIPPET  { get; set; } = string.Empty;
    [Required] public string URL_SNIPPET  { get; set; } = string.Empty;
    public string? DES_TOKEN      { get; set; }
    public bool   FLG_FREE_PAY    { get; set; }
    public string? COD_APLICACION        { get; set; }
    public string? COD_OPCION_APLICACION { get; set; }
    public string? COD_BOTON_OPCION      { get; set; }
    public int?   COD_TIPO_ESTADO { get; set; }
    public int?   COD_ESTADO      { get; set; }
}
