namespace REA.Web.Models;

public class EstadoClienteDto
{
    public string Estado { get; set; } = string.Empty;
    public int TotalClientes { get; set; }
    public int ClientesActivos { get; set; }
    public int ClientesIrregulares { get; set; }
}
