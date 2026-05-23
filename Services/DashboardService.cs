using REA.Web.Models;

namespace REA.Web.Services;

public class DashboardService
{
    private readonly ApiClient _api;

    public DashboardService(ApiClient api) => _api = api;

    public async Task<ApiResponse<DashboardDto>?> ObtenerResumenAsync(long? periodoId = null)
    {
        var query = periodoId.HasValue ? $"api/v1/dashboard/resumen?periodoId={periodoId}" : "api/v1/dashboard/resumen";
        return await _api.GetAsync<DashboardDto>(query);
    }
}
