namespace REA.Web.Models;

public class ChecklistDto
{
    public long ChecklistId { get; set; }
    public long ClienteId { get; set; }
    public string ClienteNombre { get; set; } = string.Empty;
    public long PeriodoId { get; set; }
    public string PeriodoNombre { get; set; } = string.Empty;
    public bool Completado { get; set; }
    public decimal PorcentajeAvance { get; set; }
    public bool Cerrado { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<PasoDto> Pasos { get; set; } = new();
}

public class PasoDto
{
    public long PasoId { get; set; }
    public short NumeroPaso { get; set; }
    public string NombrePaso { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public long? CompletadoPor { get; set; }
    public DateTime? CompletadoEn { get; set; }
    public string? Notas { get; set; }
    public List<EvidenciaDto> Evidencias { get; set; } = new();
}

public class EvidenciaDto
{
    public long EvidenciaId { get; set; }
    public long PasoId { get; set; }
    public string NombreArchivo { get; set; } = string.Empty;
    public string TipoMime { get; set; } = string.Empty;
    public long TamanoBytes { get; set; }
    public long SubidoPor { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateChecklistRequest
{
    public long ClienteId { get; set; }
    public long PeriodoId { get; set; }
}

public class UpdatePasoRequest
{
    public string Estado { get; set; } = string.Empty;
    public string? Notas { get; set; }
}
