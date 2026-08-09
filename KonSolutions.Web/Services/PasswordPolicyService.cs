using System.Net.Http.Json;
using System.Text.RegularExpressions;
using KONSolutions.Shared.Common;
using KONSolutions.Shared.Models.Seguridad;

namespace KONSolutions.Web.Services;

/// <summary>
/// Valida contraseñas contra las reglas definidas en la tabla SEGURIDAD.COMPLEJIDAD_PASSWORD.
/// Cada perfil de complejidad define: longitud mínima/máxima y qué tipos de caracteres exige
/// (mayúsculas, minúsculas, numéricos, especiales).
/// </summary>
public class PasswordPolicyService
{
    private readonly HttpClient _http;
    private readonly ErrorLogService _errorLog;

    public PasswordPolicyService(HttpClient http, ErrorLogService errorLog)
    { _http = http; _errorLog = errorLog; }

    /// <summary>Trae todas las reglas de complejidad (para elegir cuál aplicar).</summary>
    public async Task<List<ComplejidadPwd>> GetReglasAsync()
    {
        try
        {
            var r = await _http.GetFromJsonAsync<ApiResponse<List<ComplejidadPwd>>>(
                ApiRoutes.Seguridad.ComplejidadPassword);
            return r?.Data ?? new();
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "PasswordPolicy", nameof(GetReglasAsync)); return new(); }
    }

    /// <summary>Trae una regla específica por su código.</summary>
    public async Task<ComplejidadPwd?> GetReglaAsync(string codComplejidad)
    {
        if (string.IsNullOrWhiteSpace(codComplejidad)) return null;
        try
        {
            var r = await _http.GetFromJsonAsync<ApiResponse<ComplejidadPwd>>(
                $"{ApiRoutes.Seguridad.ComplejidadPassword}/{Uri.EscapeDataString(codComplejidad)}");
            return r?.Data;
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "PasswordPolicy", nameof(GetReglaAsync), codComplejidad); return null; }
    }

    /// <summary>
    /// Valida una contraseña contra una regla de complejidad.
    /// Devuelve la lista de errores; vacía = la contraseña cumple.
    /// </summary>
    public List<string> Validar(string password, ComplejidadPwd regla)
    {
        var errores = new List<string>();
        password ??= string.Empty;

        // Longitud
        if (regla.CAN_CARACTERES_MINIMO is int min && password.Length < min)
            errores.Add($"Debe tener al menos {min} caracteres.");
        if (regla.CAN_CARACTERES_MAXIMO is int max && max > 0 && password.Length > max)
            errores.Add($"No debe superar {max} caracteres.");

        // Tipos de caracteres requeridos: si el campo trae caracteres definidos, se exige al menos uno.
        if (Exige(regla.DES_CARACTERES_LETRAS_MAYUSCULAS) && !Regex.IsMatch(password, "[A-ZÁÉÍÓÚÑ]"))
            errores.Add("Debe incluir al menos una letra mayúscula.");
        if (Exige(regla.DES_CARACTERES_LETRAS_MINUSCULAS) && !Regex.IsMatch(password, "[a-záéíóúñ]"))
            errores.Add("Debe incluir al menos una letra minúscula.");
        if (Exige(regla.DES_CARACTERES_NUMERICOS) && !Regex.IsMatch(password, "[0-9]"))
            errores.Add("Debe incluir al menos un número.");
        if (Exige(regla.DES_CARACTERES_ESPECIALES) && !ContieneEspecial(password, regla.DES_CARACTERES_ESPECIALES!))
            errores.Add("Debe incluir al menos un carácter especial.");

        return errores;
    }

    private static bool Exige(string? caracteres) => !string.IsNullOrWhiteSpace(caracteres);

    private static bool ContieneEspecial(string password, string especiales)
    {
        // Si la regla lista los caracteres especiales permitidos, exigir al menos uno de ellos.
        // Si no, usar un set genérico.
        var set = string.IsNullOrWhiteSpace(especiales) ? "!@#$%^&*()_+-=[]{}|;:,.<>?" : especiales;
        return password.Any(c => set.Contains(c));
    }
}
