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

    public async Task<ApiResponse<List<XmlFacturaDto>>?> ListarFacturasAsync(long cargaId, int page = 1, int limit = 50)
        => await _http.GetFromJsonAsync<ApiResponse<List<XmlFacturaDto>>>($"api/v1/xml/cargas/{cargaId}/facturas?page={page}&limit={limit}");
}

public class XmlCargaDto
{
    public long CargaId { get; set; }
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
    public string RfcEmisor { get; set; } = string.Empty;
    public string? NombreEmisor { get; set; }
    public string RfcReceptor { get; set; } = string.Empty;
    public string? NombreReceptor { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Total { get; set; }
    public decimal IvaTrasladado { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string? MetodoPago { get; set; }
    public DateOnly FechaEmision { get; set; }
    public string? ErrorValidacion { get; set; }
}
