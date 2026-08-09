namespace KONSolutions.Web.Pages.Seguridad;

public class NodoAsignacionVm
{
    public string  Cod     { get; set; } = "";
    public string  Des     { get; set; } = "";
    public string? Formula { get; set; }
    public bool?   Acceso  { get; set; }
    public bool    EsApp   { get; set; }
    public string? CodRol  { get; set; }
    public List<NodoAsignacionVm>   Hijos   { get; set; } = new();
}
