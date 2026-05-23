using REA.Web.Models;

namespace REA.Web.Services;

public class ClienteService
{
    private readonly ApiClient _api;

    public ClienteService(ApiClient api) => _api = api;

    public async Task<ApiResponse<List<ClienteDto>>?> ListarAsync(string? clasificacion = null, string? busqueda = null, int page = 1, int limit = 20)
    {
        var query = $"api/v1/clientes?page={page}&limit={limit}";
        if (!string.IsNullOrEmpty(clasificacion)) query += $"&clasificacion={clasificacion}";
        if (!string.IsNullOrEmpty(busqueda)) query += $"&busqueda={busqueda}";
        return await _api.GetAsync<List<ClienteDto>>(query);
    }

    public async Task<ApiResponse<ClienteDto>?> ObtenerAsync(long id)
        => await _api.GetAsync<ClienteDto>($"api/v1/clientes/{id}");

    public async Task<ApiResponse<ClienteDto>?> CrearAsync(CreateClienteRequest request)
        => await _api.PostAsync<ClienteDto>("api/v1/clientes", request);

    public async Task<ApiResponse<ClienteDto>?> ActualizarAsync(long id, UpdateClienteRequest request)
        => await _api.PutAsync<ClienteDto>($"api/v1/clientes/{id}", request);

    public async Task DesactivarAsync(long id)
        => await _api.DeleteAsync<object>($"api/v1/clientes/{id}");
}
