using System.Net.Http.Json;
using System.Text.Json;
using REA.Web.Models;

namespace REA.Web.Services;

public class ApiClient
{
    private readonly HttpClient _http;
    private readonly TokenService _tokenService;

    public ApiClient(HttpClient http, TokenService tokenService)
    {
        _http = http;
        _tokenService = tokenService;
    }

    private async Task SetAuthHeader()
    {
        var token = await _tokenService.GetAccessTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<ApiResponse<T>?> GetAsync<T>(string url)
    {
        await SetAuthHeader();
        return await _http.GetFromJsonAsync<ApiResponse<T>>(url);
    }

    public async Task<ApiResponse<T>?> PostAsync<T>(string url, object body)
    {
        await SetAuthHeader();
        var response = await _http.PostAsJsonAsync(url, body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        return await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
    }

    public async Task<ApiResponse<T>?> PutAsync<T>(string url, object body)
    {
        await SetAuthHeader();
        var response = await _http.PutAsJsonAsync(url, body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        return await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
    }

    public async Task<ApiResponse<T>?> DeleteAsync<T>(string url)
    {
        await SetAuthHeader();
        var response = await _http.DeleteAsync(url);
        return await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
    }

    public async Task<ApiResponse<T>?> PostMultipartAsync<T>(string url, MultipartFormDataContent content)
    {
        await SetAuthHeader();
        var response = await _http.PostAsync(url, content);
        return await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
    }

    public async Task PostUnauthAsync(string url, object body)
    {
        _http.DefaultRequestHeaders.Authorization = null;
        await _http.PostAsJsonAsync(url, body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
    }

    public async Task<ApiResponse<T>?> PostUnauthAsync<T>(string url, object body)
    {
        _http.DefaultRequestHeaders.Authorization = null;
        var response = await _http.PostAsJsonAsync(url, body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        return await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
    }
}
