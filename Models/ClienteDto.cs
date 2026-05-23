namespace REA.Web.Models;

public class ClienteDto
{
    public long ClienteId { get; set; }
    public string Rfc { get; set; } = string.Empty;
    public string RazonSocial { get; set; } = string.Empty;
    public string? NombreComercial { get; set; }
    public string Clasificacion { get; set; } = string.Empty;
    public string? Actividad { get; set; }
    public decimal? PorcentajeAct { get; set; }
    public string? RegimenFiscal { get; set; }
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public DateOnly? EFirmaVigenciaInicio { get; set; }
    public DateOnly? EFirmaVigenciaFin { get; set; }
    public DateOnly? CieeVigenciaInicio { get; set; }
    public DateOnly? CieeVigenciaFin { get; set; }
    public string? EstatusEfirma { get; set; }
    public string? Notas { get; set; }
    public bool Activo { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<AsignacionDto> Asignaciones { get; set; } = new();
}

public class AsignacionDto
{
    public long AsignacionId { get; set; }
    public long AuxiliarId { get; set; }
    public string AuxiliarNombre { get; set; } = string.Empty;
}

public class CreateClienteRequest
{
    public string Rfc { get; set; } = string.Empty;
    public string RazonSocial { get; set; } = string.Empty;
    public string? NombreComercial { get; set; }
    public string Clasificacion { get; set; } = "activo";
    public string? Actividad { get; set; }
    public decimal? PorcentajeAct { get; set; }
    public string? RegimenFiscal { get; set; }
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
}

public class UpdateClienteRequest
{
    public string? RazonSocial { get; set; }
    public string? NombreComercial { get; set; }
    public string? Clasificacion { get; set; }
    public string? Actividad { get; set; }
    public decimal? PorcentajeAct { get; set; }
    public string? RegimenFiscal { get; set; }
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
}
