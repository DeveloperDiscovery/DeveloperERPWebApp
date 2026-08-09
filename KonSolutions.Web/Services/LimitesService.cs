using Blazored.LocalStorage;
using System.Net.Http.Json;
using KONSolutions.Shared.Common;
using KONSolutions.Shared.Models.Entorno;

namespace KONSolutions.Web.Services;

/// <summary>
/// Servicio que carga los límites de las columnas de la BD (una vez al iniciar)
/// y ofrece helpers para que cada campo aplique el límite que le corresponde:
///   - Texto:    MaxLength(tabla, columna)
///   - Enteros:  Min(tabla, columna) / Max(tabla, columna)
///   - Decimales: Decimales(tabla, columna)  y  MaxDecimal(tabla, columna)
///
/// "tabla" se pasa como "SCHEMA.TABLA" (ej. "SEGURIDAD.USUARIOS").
/// </summary>
public class LimitesService
{
    private readonly HttpClient           _http;
    private readonly ErrorLogService      _errorLog;
    private readonly ILocalStorageService _localStorage;

    private readonly Dictionary<string, LimiteColumna> _limites = new(StringComparer.OrdinalIgnoreCase);
    private bool _cargado;

    public LimitesService(HttpClient http, ErrorLogService errorLog, ILocalStorageService localStorage)
    {
        _http         = http;
        _errorLog     = errorLog;
        _localStorage = localStorage;
    }

    /// <summary>
    /// Carga los límites desde la API. Solo ejecuta si hay sesión activa.
    /// </summary>
    public async Task CargarAsync()
    {
        if (_cargado) return;
        try
        {
            var sesion = await _localStorage.GetItemAsync<object>("usuarioSesion");
            if (sesion is null) return;

            var r = await _http.GetFromJsonAsync<ApiResponse<List<LimiteColumna>>>(
                ApiRoutes.Entorno.LimitesColumnas);
            if (r?.Data is not null)
            {
                foreach (var c in r.Data)
                {
                    var clave = $"{c.TABLE_SCHEMA}.{c.TABLE_NAME}.{c.COLUMN_NAME}";
                    _limites[clave] = c;
                }
                _cargado = true;
            }
        }
        catch (Exception ex) { await _errorLog.LogAsync(ex, "Limites", nameof(CargarAsync)); }
    }

    /// <summary>Busca el límite de una columna. "tabla" = "SCHEMA.TABLA".</summary>
    private LimiteColumna? Buscar(string tabla, string columna)
        => _limites.TryGetValue($"{tabla}.{columna}", out var l) ? l : null;

    /// <summary>Longitud máxima para un campo de texto (null si no aplica).</summary>
    public int? MaxLength(string tabla, string columna)
        => Buscar(tabla, columna)?.CHARACTER_MAXIMUM_LENGTH;

    /// <summary>
    /// Ancho CSS proporcional al CHARACTER_MAXIMUM_LENGTH de la columna.
    /// Útil para dimensionar columnas de MudDataGrid según el tamaño real del campo en la BD.
    /// Escala: clamp(maxLength * 5 + 40, minPx, maxPx).
    /// </summary>
    public string ColWidth(string tabla, string columna, int minPx = 60, int maxPx = 500)
    {
        var max = MaxLength(tabla, columna);
        if (max is null or <= 0) return $"{minPx}px";
        var px = Math.Clamp(max.Value * 5 + 40, minPx, maxPx);
        return $"{px}px";
    }

    /// <summary>
    /// Indica si la columna es obligatoria (NOT NULL en la BD).
    /// Permite marcar automáticamente los campos requeridos en los CRUD (v3).
    /// </summary>
    public bool EsObligatorio(string tabla, string columna)
        => string.Equals(Buscar(tabla, columna)?.IS_NULLABLE, "NO", StringComparison.OrdinalIgnoreCase);

    /// <summary>Valor mínimo para un campo entero (según el tipo SQL).</summary>
    public long? Min(string tabla, string columna)
    {
        var l = Buscar(tabla, columna);
        if (l is null) return null;
        return l.DATA_TYPE.ToLower() switch
        {
            "tinyint" => 0,
            "smallint" => -32768,
            "int" => -2147483648,
            "bigint" => long.MinValue,
            _ => null
        };
    }

    /// <summary>Valor máximo para un campo entero (según el tipo SQL).</summary>
    public long? Max(string tabla, string columna)
    {
        var l = Buscar(tabla, columna);
        if (l is null) return null;
        return l.DATA_TYPE.ToLower() switch
        {
            "tinyint" => 255,
            "smallint" => 32767,
            "int" => 2147483647,
            "bigint" => long.MaxValue,
            _ => null
        };
    }

    /// <summary>Cantidad de decimales permitidos (escala) para decimal/numeric/money.</summary>
    public int? Decimales(string tabla, string columna)
    {
        var l = Buscar(tabla, columna);
        if (l is null) return null;
        return l.DATA_TYPE.ToLower() switch
        {
            "decimal" or "numeric" => l.NUMERIC_SCALE,
            "money" or "smallmoney" => 4,
            _ => null
        };
    }

    /// <summary>
    /// Valor máximo para un decimal según precisión y escala.
    /// Ej. decimal(10,2) → 99999999.99
    /// </summary>
    public decimal? MaxDecimal(string tabla, string columna)
    {
        var l = Buscar(tabla, columna);
        if (l is null) return null;
        var tipo = l.DATA_TYPE.ToLower();
        if (tipo is not ("decimal" or "numeric")) return null;
        if (l.NUMERIC_PRECISION is null) return null;

        var precision = l.NUMERIC_PRECISION.Value;
        var escala = l.NUMERIC_SCALE ?? 0;
        var enteros = precision - escala;
        // 10^enteros - 10^-escala  (ej. 8 enteros, 2 dec → 99999999.99)
        var maxEntero = (decimal)Math.Pow(10, enteros) - (decimal)Math.Pow(10, -escala);
        return maxEntero;
    }

    /// <summary>Indica si una columna es de tipo texto.</summary>
    public bool EsTexto(string tabla, string columna)
    {
        var t = Buscar(tabla, columna)?.DATA_TYPE.ToLower();
        return t is "varchar" or "nvarchar" or "char" or "nchar";
    }
}
