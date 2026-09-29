using System.Data.OleDb;

namespace MantenimientoTextil.Infraestructura.Access;

public class ConexionAccess(string rutaAccdb)
{
    // Requiere Microsoft.ACE.OLEDB (compilar x64 o x86 según el Office instalado).
    public OleDbConnection Crear() =>
        new($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={rutaAccdb};");
}
