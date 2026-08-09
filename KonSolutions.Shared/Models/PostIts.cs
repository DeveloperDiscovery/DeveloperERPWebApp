namespace KONSolutions.Shared.Models.Entorno;

/// <summary>Nota adhesiva personal (ENTORNO.POSTITS).</summary>
public class PostIt
{
    public long NUM_POSTIT { get; set; }
    public string COD_USUARIO { get; set; } = string.Empty;
    public string DES_TITULO { get; set; } = string.Empty;
    public string DES_POSTIT { get; set; } = string.Empty;
    public int POS_LEFT { get; set; }
    public int POS_TOP { get; set; }
}
