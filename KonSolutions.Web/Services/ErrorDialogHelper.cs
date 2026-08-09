using System.Linq;
using KONSolutions.Shared.Common;
using MudBlazor;

namespace KONSolutions.Web.Services;

/// <summary>
/// Mismo tratamiento que los errores de SQL (ErrorSqlDialog) pero reutilizable
/// desde cualquier diálogo que hoy solo muestra un Snackbar genérico
/// ("Error al guardar."/"Error al eliminar.") sin detalle y sin registrar nada.
///
/// Uso típico:
///   var resp = await Http.PostAsJsonAsync(ruta, _model);
///   if (resp.IsSuccessStatusCode) { ... }
///   else await ErrorDialogHelper.MostrarAsync(resp, ErrorLog, DialogService, nameof(MiDialogo), nameof(Submit));
/// </summary>
public static class ErrorDialogHelper
{
    public static async Task MostrarAsync(
        HttpResponseMessage resp, ErrorLogService errorLog, IDialogService dialogService,
        string pagina, string metodo, string? titulo = null)
    {
        // 401 = sesión expirada: JwtMessageHandler YA lo detectó y está redirigiendo a
        // /login (NavigateTo forceLoad:true) — como esa redirección no corta sincrónicamente
        // la ejecución en curso, sin este guard el diálogo de "no se pudo guardar" alcanzaba
        // a mostrarse igual, justo antes de que la página recargara al login. Se ignora acá
        // para no confundir un token vencido con un rechazo real del servidor.
        if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized) return;

        string mensaje;
        int? errorNumber = null;
        string? errorProcedure = null;
        int? errorLine = null;
        string? sqlSentence = null;
        var body = await resp.Content.ReadAsStringAsync();

        try
        {
            // Caso 1: el backend devolvió un WriteResult con detalle SQL (ErrorNumber/ErrorProcedure/ErrorLine).
            var apiErr = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<object>>(body,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (apiErr is not null && (!string.IsNullOrWhiteSpace(apiErr.Message) || apiErr.ErrorNumber.HasValue))
            {
                mensaje = apiErr.Message ?? "Error al guardar.";
                errorNumber    = apiErr.ErrorNumber;
                errorProcedure = apiErr.ErrorProcedure;
                errorLine      = apiErr.ErrorLine;
                sqlSentence    = apiErr.SqlSentence;
            }
            else
            {
                // Caso 2: 400 de ModelState (ValidationProblemDetails) — junta los mensajes de cada campo.
                using var doc = System.Text.Json.JsonDocument.Parse(body);
                if (doc.RootElement.TryGetProperty("errors", out var errores))
                {
                    var partes = new List<string>();
                    foreach (var campo in errores.EnumerateObject())
                        foreach (var msg in campo.Value.EnumerateArray())
                            partes.Add(msg.GetString() ?? "");
                    mensaje = partes.Count > 0 ? string.Join(" · ", partes) : body;
                }
                else mensaje = body;
            }
        }
        catch { mensaje = string.IsNullOrWhiteSpace(body) ? $"HTTP {(int)resp.StatusCode}" : body; }

        // Origen: SQL si el rechazo trae ErrorNumber (el SP lo lanzó); si no, es un rechazo
        // de la API (.NET) — validación, ModelState, etc. — sin pasar por un SP.
        var origen = errorNumber.HasValue ? OrigenError.SQL : OrigenError.API;

        await errorLog.LogRechazoAsync(mensaje, pagina, metodo, body);

        await dialogService.ShowAsync<KONSolutions.Web.Pages.Shared.ErrorSqlDialog>("",
            new DialogParameters
            {
                ["Titulo"] = titulo ?? "No se pudo guardar el registro", ["Mensaje"] = mensaje, ["Origen"] = origen,
                ["ErrorNumber"] = errorNumber, ["ErrorProcedure"] = errorProcedure, ["ErrorLine"] = errorLine,
                ["SqlSentence"] = sqlSentence, ["Pagina"] = pagina
            },
            new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true, CloseButton = false });
    }

    /// <summary>
    /// Igual que MostrarAsync, pero para los servicios (MotivoService, CatalogoService&lt;,&gt;, etc.)
    /// que ya devuelven el mensaje extraído como string (no exponen el HttpResponseMessage crudo).
    /// El mensaje ya fue registrado por el servicio antes de llegar acá, así que no se vuelve a loguear.
    ///
    /// Uso típico:
    ///   var (ok, msg) = await Service.GuardarAsync(_model);
    ///   if (ok) { ... } else await ErrorDialogHelper.MostrarMensajeAsync(msg, DialogService, nameof(MiDialogo));
    /// </summary>
    public static Task MostrarMensajeAsync(
        string mensaje, IDialogService dialogService, string pagina, string? titulo = null)
    {
        // Mismo caso que en MostrarAsync(HttpResponseMessage): si la excepción que llegó acá
        // viene de un GetFromJsonAsync/EnsureSuccessStatusCode que reventó por un 401, el
        // JwtMessageHandler YA detectó el token vencido y está redirigiendo a /login — mostrar
        // este diálogo encima solo confunde al usuario justo antes de que la página recargue.
        if (EsSesionExpirada(mensaje)) return Task.CompletedTask;

        return dialogService.ShowAsync<KONSolutions.Web.Pages.Shared.ErrorSqlDialog>("",
            new DialogParameters
            {
                ["Titulo"] = titulo ?? "No se pudo guardar el registro", ["Mensaje"] = mensaje,
                ["Origen"] = OrigenError.SQL, ["Pagina"] = pagina
            },
            new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true, CloseButton = false });
    }

    /// <summary>
    /// Para cuando falla la validación de un <EditForm EditContext="..."> ANTES de llegar
    /// a la API (campos obligatorios vacíos, etc.) — mismo diálogo estándar que los errores
    /// de SQL/API, en vez del <ValidationSummary> crudo (rojo, en inglés, sin estilo propio).
    /// No pasa por el log de errores: no es un rechazo del servidor, es un chequeo local.
    ///
    /// Uso típico (footer fuera del EditForm, ver TipoOperacionDialog.razor):
    ///   if (!_editContext.Validate())
    ///   { await ErrorDialogHelper.MostrarValidacionAsync(_editContext, DialogService, nameof(MiDialogo)); return; }
    /// </summary>
    private static readonly System.Text.RegularExpressions.Regex _reRequired =
        new(@"^The (.+) field is required\.$", System.Text.RegularExpressions.RegexOptions.Compiled);

    /// <summary>Traduce el mensaje default en inglés de DataAnnotations ("The X field is
    /// required.") a español — cubre el caso más común de [Required] sin ErrorMessage propio.</summary>
    private static string Traducir(string mensaje)
    {
        var m = _reRequired.Match(mensaje);
        return m.Success ? $"El campo \"{m.Groups[1].Value}\" es obligatorio." : mensaje;
    }

    /// <summary>
    /// Detecta si un mensaje de excepción corresponde a un 401 (token vencido). Cubre tanto
    /// el mensaje default de HttpRequestException ("Response status code does not indicate
    /// success: 401 (Unauthorized).") como cualquier texto que ya incluya el código armado a mano.
    /// </summary>
    private static bool EsSesionExpirada(string mensaje) =>
        !string.IsNullOrEmpty(mensaje) &&
        mensaje.Contains("401") &&
        mensaje.Contains("Unauthorized", StringComparison.OrdinalIgnoreCase);

    public static Task MostrarValidacionAsync(
        Microsoft.AspNetCore.Components.Forms.EditContext editContext, IDialogService dialogService, string pagina)
    {
        var mensaje = string.Join(" · ", editContext.GetValidationMessages().Select(Traducir));
        return dialogService.ShowAsync<KONSolutions.Web.Pages.Shared.ErrorSqlDialog>("",
            new DialogParameters
            {
                ["Titulo"] = "Faltan datos obligatorios", ["Mensaje"] = mensaje,
                ["Origen"] = OrigenError.API, ["Pagina"] = pagina
            },
            new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true, CloseButton = false });
    }
}
