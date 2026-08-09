using System.Globalization;

namespace KONSolutions.Web.Services;

/// <summary>
/// Aplica la cultura elegida en Configuración General (fechas, nombres de mes, etc.)
/// pero forzando SIEMPRE el formato numérico a estilo US (coma de miles, punto decimal)
/// sin importar qué código de cultura (es-PE, es-ES, ...) se haya seleccionado — el
/// admin puede cambiar la cultura regional libremente y los números de toda la web
/// no deben variar.
/// </summary>
public static class CultureHelper
{
    public static CultureInfo Construir(string codigoCultura)
    {
        var ci = (CultureInfo)new CultureInfo(codigoCultura).Clone();
        ci.NumberFormat.NumberGroupSeparator = ",";
        ci.NumberFormat.NumberDecimalSeparator = ".";
        ci.NumberFormat.CurrencyGroupSeparator = ",";
        ci.NumberFormat.CurrencyDecimalSeparator = ".";
        return ci;
    }

    public static void Aplicar(string codigoCultura)
    {
        var ci = Construir(codigoCultura);
        CultureInfo.DefaultThreadCurrentCulture   = ci;
        CultureInfo.DefaultThreadCurrentUICulture = ci;
        CultureInfo.CurrentCulture   = ci;
        CultureInfo.CurrentUICulture = ci;
    }
}
