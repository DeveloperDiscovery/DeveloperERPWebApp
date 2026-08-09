namespace KONSolutions.Shared.Models.Entorno;

/// <summary>Archivo físico dentro de una carpeta de documentos genérica (ver
/// DocumentosCarpetaPanel.razor / DocumentosCarpetaController). No hay tabla de metadatos:
/// se lista directamente el contenido del directorio en disco.</summary>
public class DocumentoCarpetaDto
{
    public string   NombreArchivo      { get; set; } = string.Empty;
    public long     TamanoBytes        { get; set; }
    public DateTime FechaModificacion  { get; set; }
    public string   Extension          { get; set; } = string.Empty;
    public string   RutaCompleta       { get; set; } = string.Empty;
}
