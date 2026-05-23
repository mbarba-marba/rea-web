namespace REA.Web.Components.Checklist;

public class EvidenciaItem
{
    public long EvidenciaId { get; set; }
    public string NombreArchivo { get; set; } = string.Empty;
    public string TipoMime { get; set; } = string.Empty;
    public long TamanoBytes { get; set; }
}
