namespace REA.Web.Models;

public class DashboardDto
{
    public int TotalClientes { get; set; }
    public int ClientesActivos { get; set; }
    public int ClientesIrregulares { get; set; }
    public int ChecklistsCompletados { get; set; }
    public int ChecklistsEnProceso { get; set; }
    public int ChecklistsPendientes { get; set; }
    public decimal PorcentajeGlobalAvance { get; set; }
    public int DeclaracionesVencidas { get; set; }
}

public class PeriodoDto
{
    public long PeriodoId { get; set; }
    public int Anio { get; set; }
    public int Mes { get; set; }
    public DateOnly FechaInicio { get; set; }
    public DateOnly FechaFin { get; set; }
    public bool Cerrado { get; set; }
}
