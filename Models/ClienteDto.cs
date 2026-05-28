namespace REA.Web.Models;

public class ClienteDto
{
    public long ClienteId { get; set; }
    public string Rfc { get; set; } = string.Empty;
    public string RazonSocial { get; set; } = string.Empty;
    public string? DenominacionONombreFiscal { get; set; }
    public string? NombreComercial { get; set; }
    public string Clasificacion { get; set; } = string.Empty;
    public string? Actividad { get; set; }
    public decimal? PorcentajeAct { get; set; }
    public string? RegimenFiscal { get; set; }
    public DateOnly? FechaInicioOperaciones { get; set; }
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public string? Pais { get; set; }
    public string? Estado { get; set; }
    public string? Municipio { get; set; }
    public string? Localidad { get; set; }
    public string? Colonia { get; set; }
    public string? CodigoPostal { get; set; }
    public string? TipoVialidad { get; set; }
    public string? NombreVialidad { get; set; }
    public string? NumeroExterior { get; set; }
    public string? NumeroInterior { get; set; }
    public string? EntreCalle { get; set; }
    public string? YCalle { get; set; }
    public DateOnly? EFirmaVigenciaInicio { get; set; }
    public DateOnly? EFirmaVigenciaFin { get; set; }
    public DateOnly? CieeVigenciaInicio { get; set; }
    public DateOnly? CieeVigenciaFin { get; set; }
    public string? EstatusEfirma { get; set; }
    public string? Notas { get; set; }
    public bool Activo { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<AsignacionDto> Asignaciones { get; set; } = new();
    public List<ClienteRegimenDto> Regimenes { get; set; } = new();
    public List<ClienteActividadEconomicaDto> ActividadesEconomicas { get; set; } = new();
    public List<ClienteObligacionFiscalDto> ObligacionesFiscales { get; set; } = new();
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
    public string? DenominacionONombreFiscal { get; set; }
    public string? NombreComercial { get; set; }
    public string Clasificacion { get; set; } = "activo";
    public string? Actividad { get; set; }
    public decimal? PorcentajeAct { get; set; }
    public string? RegimenFiscal { get; set; }
    public DateOnly? FechaInicioOperaciones { get; set; }
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public string? Pais { get; set; }
    public string? Estado { get; set; }
    public string? Municipio { get; set; }
    public string? Localidad { get; set; }
    public string? Colonia { get; set; }
    public string? CodigoPostal { get; set; }
    public string? TipoVialidad { get; set; }
    public string? NombreVialidad { get; set; }
    public string? NumeroExterior { get; set; }
    public string? NumeroInterior { get; set; }
    public string? EntreCalle { get; set; }
    public string? YCalle { get; set; }
    public List<ClienteRegimenDto> Regimenes { get; set; } = new();
    public List<ClienteActividadEconomicaDto> ActividadesEconomicas { get; set; } = new();
    public List<ClienteObligacionFiscalDto> ObligacionesFiscales { get; set; } = new();
}

public class UpdateClienteRequest
{
    public string? RazonSocial { get; set; }
    public string? DenominacionONombreFiscal { get; set; }
    public string? NombreComercial { get; set; }
    public string? Clasificacion { get; set; }
    public string? Actividad { get; set; }
    public decimal? PorcentajeAct { get; set; }
    public string? RegimenFiscal { get; set; }
    public DateOnly? FechaInicioOperaciones { get; set; }
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public string? Pais { get; set; }
    public string? Estado { get; set; }
    public string? Municipio { get; set; }
    public string? Localidad { get; set; }
    public string? Colonia { get; set; }
    public string? CodigoPostal { get; set; }
    public string? TipoVialidad { get; set; }
    public string? NombreVialidad { get; set; }
    public string? NumeroExterior { get; set; }
    public string? NumeroInterior { get; set; }
    public string? EntreCalle { get; set; }
    public string? YCalle { get; set; }
    public List<ClienteRegimenDto>? Regimenes { get; set; }
    public List<ClienteActividadEconomicaDto>? ActividadesEconomicas { get; set; }
    public List<ClienteObligacionFiscalDto>? ObligacionesFiscales { get; set; }
}

public class ClienteRegimenDto
{
    public long ClienteRegimenId { get; set; }
    public string NombreRegimen { get; set; } = string.Empty;
    public DateOnly? FechaInicio { get; set; }
    public DateOnly? FechaFin { get; set; }
}

public class ClienteActividadEconomicaDto
{
    public long ClienteActividadEconomicaId { get; set; }
    public short Orden { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal? Porcentaje { get; set; }
    public DateOnly? FechaInicio { get; set; }
    public DateOnly? FechaFin { get; set; }
}

public class ClienteObligacionFiscalDto
{
    public long ClienteObligacionFiscalId { get; set; }
    public string DescripcionObligacion { get; set; } = string.Empty;
    public string? DescripcionVencimiento { get; set; }
    public DateOnly? FechaInicio { get; set; }
    public DateOnly? FechaFin { get; set; }
}

public class CsfExtractionDto
{
    public string? Rfc { get; set; }
    public string? RazonSocial { get; set; }
    public string? DenominacionONombreFiscal { get; set; }
    public string? NombreComercial { get; set; }
    public DateOnly? FechaInicioOperaciones { get; set; }
    public string? Pais { get; set; }
    public string? Estado { get; set; }
    public string? Municipio { get; set; }
    public string? Localidad { get; set; }
    public string? Colonia { get; set; }
    public string? CodigoPostal { get; set; }
    public string? TipoVialidad { get; set; }
    public string? NombreVialidad { get; set; }
    public string? NumeroExterior { get; set; }
    public string? NumeroInterior { get; set; }
    public string? EntreCalle { get; set; }
    public string? YCalle { get; set; }
    public List<ClienteRegimenDto> Regimenes { get; set; } = new();
    public List<ClienteActividadEconomicaDto> ActividadesEconomicas { get; set; } = new();
    public List<ClienteObligacionFiscalDto> ObligacionesFiscales { get; set; } = new();
}
