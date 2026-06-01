using System.Net.Http.Json;
using REA.Web.Models;

namespace REA.Web.Services;

public class XmlService
{
    private readonly HttpClient _http;

    public XmlService(HttpClient http) => _http = http;

    public async Task<ApiResponse<XmlCargaDto>?> SubirCargaAsync(MultipartFormDataContent content)
    {
        var response = await _http.PostAsync("api/v1/xml/cargas", content);
        return await response.Content.ReadFromJsonAsync<ApiResponse<XmlCargaDto>>();
    }

    public async Task<ApiResponse<List<XmlCargaDto>>?> ListarCargasAsync(long? clienteId = null, long? periodoId = null, string? tipoCarga = null, int page = 1, int limit = 10)
    {
        var query = $"api/v1/xml/cargas?page={page}&limit={limit}";
        if (clienteId.HasValue) query += $"&clienteId={clienteId}";
        if (periodoId.HasValue) query += $"&periodoId={periodoId}";
        if (!string.IsNullOrWhiteSpace(tipoCarga)) query += $"&tipoCarga={Uri.EscapeDataString(tipoCarga)}";
        return await _http.GetFromJsonAsync<ApiResponse<List<XmlCargaDto>>>(query);
    }

    public async Task<ApiResponse<XmlCargaDto>?> ObtenerCargaAsync(long id)
        => await _http.GetFromJsonAsync<ApiResponse<XmlCargaDto>>($"api/v1/xml/cargas/{id}");

    public async Task<ApiResponse<List<XmlCargaDto>>?> ListarCargasPorChecklistAsync(long checklistId, string? tipoCarga = null, int page = 1, int limit = 50)
    {
        var query = $"api/v1/xml/checklists/{checklistId}/cargas?page={page}&limit={limit}";
        if (!string.IsNullOrWhiteSpace(tipoCarga)) query += $"&tipoCarga={Uri.EscapeDataString(tipoCarga)}";
        return await _http.GetFromJsonAsync<ApiResponse<List<XmlCargaDto>>>(query);
    }

    public async Task<ApiResponse<List<XmlFacturaDto>>?> ListarFacturasAsync(long cargaId, int page = 1, int limit = 50)
        => await _http.GetFromJsonAsync<ApiResponse<List<XmlFacturaDto>>>($"api/v1/xml/cargas/{cargaId}/facturas?page={page}&limit={limit}");

    public async Task<ApiResponse<List<XmlFacturaDto>>?> BuscarFacturasAsync(
        long? clienteId = null,
        long? periodoId = null,
        int? anio = null,
        string? tipo = null,
        string? metodoPago = null,
        int page = 1,
        int limit = 50)
    {
        var query = $"api/v1/xml/facturas?page={page}&limit={limit}";
        if (clienteId.HasValue) query += $"&clienteId={clienteId}";
        if (periodoId.HasValue) query += $"&periodoId={periodoId}";
        if (anio.HasValue) query += $"&anio={anio}";
        if (!string.IsNullOrWhiteSpace(tipo)) query += $"&tipo={Uri.EscapeDataString(tipo)}";
        if (!string.IsNullOrWhiteSpace(metodoPago)) query += $"&metodoPago={Uri.EscapeDataString(metodoPago)}";
        return await _http.GetFromJsonAsync<ApiResponse<List<XmlFacturaDto>>>(query);
    }

    public async Task<ApiResponse<List<XmlFacturaDto>>?> ListarFacturasPorChecklistAsync(long checklistId, string? tipo = null, string? metodoPago = null, int page = 1, int limit = 200)
    {
        var query = $"api/v1/xml/checklists/{checklistId}/facturas?page={page}&limit={limit}";
        if (!string.IsNullOrWhiteSpace(tipo)) query += $"&tipo={Uri.EscapeDataString(tipo)}";
        if (!string.IsNullOrWhiteSpace(metodoPago)) query += $"&metodoPago={Uri.EscapeDataString(metodoPago)}";
        return await _http.GetFromJsonAsync<ApiResponse<List<XmlFacturaDto>>>(query);
    }

    public async Task<ApiResponse<List<XmlFacturaDto>>?> ObtenerFacturasPendientesCategoriaAsync(long clienteId, long periodoId)
        => await _http.GetFromJsonAsync<ApiResponse<List<XmlFacturaDto>>>($"api/v1/xml/facturas/pendientes-categoria?clienteId={clienteId}&periodoId={periodoId}");

    public async Task<ApiResponse<List<XmlFacturaDto>>?> ObtenerFacturasPendientesCategoriaPorChecklistAsync(long checklistId)
        => await _http.GetFromJsonAsync<ApiResponse<List<XmlFacturaDto>>>($"api/v1/xml/checklists/{checklistId}/facturas/pendientes-categoria");

    public async Task<ApiResponse<List<XmlFacturaDto>>?> ActualizarCategoriasFacturasAsync(ActualizarCategoriasFacturasRequest request)
    {
        var response = await _http.PutAsJsonAsync("api/v1/xml/facturas/categorias", request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<List<XmlFacturaDto>>>();
    }

    public async Task<ApiResponse<List<CategoriaEgresoXmlConfigDto>>?> ObtenerConfiguracionCategoriasAsync()
        => await _http.GetFromJsonAsync<ApiResponse<List<CategoriaEgresoXmlConfigDto>>>("api/v1/xml/categorias-egresos-config");

    public async Task<ApiResponse<List<CategoriaEgresoXmlConfigDto>>?> ActualizarConfiguracionCategoriasAsync(ActualizarCategoriasEgresosXmlConfigRequest request)
    {
        var response = await _http.PutAsJsonAsync("api/v1/xml/categorias-egresos-config", request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<List<CategoriaEgresoXmlConfigDto>>>();
    }

    public async Task<ApiResponse<object>?> EliminarFacturaAsync(long facturaId)
    {
        var response = await _http.DeleteAsync($"api/v1/xml/facturas/{facturaId}");
        return await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
    }
}

public class XmlCargaDto
{
    public long CargaId { get; set; }
    public long? ChecklistId { get; set; }
    public long? PasoId { get; set; }
    public long ClienteId { get; set; }
    public string ClienteNombre { get; set; } = string.Empty;
    public long PeriodoId { get; set; }
    public string TipoCarga { get; set; } = string.Empty;
    public string NombreArchivo { get; set; } = string.Empty;
    public string NombreZip { get; set; } = string.Empty;
    public long? TamanoBytes { get; set; }
    public int TotalArchivos { get; set; }
    public int Procesados { get; set; }
    public int ConErrores { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string? ErrorMensaje { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcesadoEn { get; set; }
}

public class XmlFacturaDto
{
    public long FacturaId { get; set; }
    public string Uuid { get; set; } = string.Empty;
    public string TipoCarga { get; set; } = string.Empty;
    public string RfcEmisor { get; set; } = string.Empty;
    public string? NombreEmisor { get; set; }
    public string RfcReceptor { get; set; } = string.Empty;
    public string? NombreReceptor { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Descuento { get; set; }
    public decimal SubNeto { get; set; }
    public decimal Total { get; set; }
    public decimal IvaTrasladado { get; set; }
    public decimal IepsTrasladado { get; set; }
    public decimal IvaRetenido { get; set; }
    public decimal IsrRetenido { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string? MetodoPago { get; set; }
    public string? CategoriaGastoXml { get; set; }
    public long? CategoriaAsignadaPor { get; set; }
    public DateTime? CategoriaAsignadaEn { get; set; }
    public bool TieneComplemento { get; set; }
    public string? ComplementosDetectados { get; set; }
    public DateOnly FechaEmision { get; set; }
    public DateOnly? FechaPago { get; set; }
    public decimal? MontoPago { get; set; }
    public DateTime ProcesadoEn { get; set; }
    public bool EsValida { get; set; }
    public string? FormaPago { get; set; }
    public string Moneda { get; set; } = "MXN";
    public string? ErrorValidacion { get; set; }
    public List<XmlFacturaImpuestoDto> Impuestos { get; set; } = new();
}

public class CategoriaEgresoXmlConfigDto
{
    public string Categoria { get; set; } = string.Empty;
    public string IsrDestino { get; set; } = string.Empty;
    public decimal IsrPorcentajeDeducible { get; set; }
    public string IvaModoAcreditamiento { get; set; } = string.Empty;
    public decimal IvaPorcentajeAcreditable { get; set; }
    public bool ActivaParaReporte { get; set; }
}

public class ActualizarCategoriasFacturasRequest
{
    public long ClienteId { get; set; }
    public long PeriodoId { get; set; }
    public List<ActualizarCategoriaFacturaItemDto> Items { get; set; } = new();
}

public class ActualizarCategoriaFacturaItemDto
{
    public long FacturaId { get; set; }
    public string? Categoria { get; set; }
}

public class ActualizarCategoriasEgresosXmlConfigRequest
{
    public List<ActualizarCategoriaEgresoXmlConfigItemDto> Items { get; set; } = new();
}

public class ActualizarCategoriaEgresoXmlConfigItemDto
{
    public string Categoria { get; set; } = string.Empty;
    public string IsrDestino { get; set; } = string.Empty;
    public decimal IsrPorcentajeDeducible { get; set; }
    public string IvaModoAcreditamiento { get; set; } = string.Empty;
    public decimal IvaPorcentajeAcreditable { get; set; }
    public bool ActivaParaReporte { get; set; }
}

public class XmlFacturaImpuestoDto
{
    public string Tipo { get; set; } = string.Empty;
    public string ClaveImpuesto { get; set; } = string.Empty;
    public string NombreImpuesto { get; set; } = string.Empty;
    public string? TipoFactor { get; set; }
    public decimal? Base { get; set; }
    public decimal? TasaOCuota { get; set; }
    public decimal Importe { get; set; }
}
