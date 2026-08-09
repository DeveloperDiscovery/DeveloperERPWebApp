namespace KONSolutions.Web.Services;

/// <summary>Origen de un error mostrado en ErrorSqlBox.razor/ErrorSqlDialog.razor:
/// SQL (el SP lanzó el error, viene con ErrorNumber), API (excepción/validación del
/// backend .NET sin pasar por un SP) o WEB (validación o excepción del propio navegador).</summary>
public enum OrigenError { SQL, API, WEB }
