using REA.Web.Models;

namespace REA.Web.Services;

public class ChecklistService
{
    private readonly ApiClient _api;

    public ChecklistService(ApiClient api) => _api = api;

    public async Task<ApiResponse<List<ChecklistDto>>?> ListarAsync(long? clienteId = null, long? periodoId = null, int page = 1, int limit = 20)
    {
        var query = $"api/v1/checklists?page={page}&limit={limit}";
        if (clienteId.HasValue) query += $"&clienteId={clienteId}";
        if (periodoId.HasValue) query += $"&periodoId={periodoId}";
        return await _api.GetAsync<List<ChecklistDto>>(query);
    }

    public async Task<ApiResponse<ChecklistDto>?> ObtenerAsync(long id)
        => await _api.GetAsync<ChecklistDto>($"api/v1/checklists/{id}");

    public async Task<ApiResponse<ChecklistDto>?> CrearAsync(CreateChecklistRequest request)
        => await _api.PostAsync<ChecklistDto>("api/v1/checklists", request);

    public async Task<ApiResponse<PasoDto>?> ActualizarPasoAsync(long checklistId, long pasoId, UpdatePasoRequest request)
        => await _api.PutAsync<PasoDto>($"api/v1/checklists/{checklistId}/pasos/{pasoId}", request);

    public async Task<ApiResponse<ProtocoloChecklistResponseDto>?> ObtenerProtocoloAsync()
        => await _api.GetAsync<ProtocoloChecklistResponseDto>("api/v1/checklists/protocolo");

    public async Task<ApiResponse<ProtocoloChecklistResponseDto>?> ActualizarProtocoloAsync(UpdateProtocoloChecklistRequest request)
        => await _api.PutAsync<ProtocoloChecklistResponseDto>("api/v1/checklists/protocolo", request);
}
