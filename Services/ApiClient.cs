using System.Net.Http.Json;
using System.Text.Json;
using REA.Web.Models;

namespace REA.Web.Services;

public class ApiClient
{
    private readonly HttpClient _http;
    private readonly TokenService _tokenService;
    private readonly SessionRedirectService _sessionRedirect;

    public ApiClient(HttpClient http, TokenService tokenService, SessionRedirectService sessionRedirect)
    {
        _http = http;
        _tokenService = tokenService;
        _sessionRedirect = sessionRedirect;
    }

    private async Task<bool> SetAuthHeader()
    {
        var token = await _tokenService.GetAccessTokenAsync();
        if (_tokenService.IsTokenExpired(token))
        {
            _http.DefaultRequestHeaders.Authorization = null;
            await _sessionRedirect.RedirectToLoginAsync();
            return false;
        }

        if (!string.IsNullOrEmpty(token))
        {
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        return true;
    }

    private async Task<ApiResponse<T>?> ReadResponseAsync<T>(HttpResponseMessage response)
    {
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            await _sessionRedirect.RedirectToLoginAsync();
            return default;
        }

        return await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
    }

    public async Task<ApiResponse<T>?> GetAsync<T>(string url)
    {
        if (!await SetAuthHeader())
        {
            return default;
        }

        var response = await _http.GetAsync(url);
        return await ReadResponseAsync<T>(response);
    }

    public async Task<ApiResponse<T>?> PostAsync<T>(string url, object body)
    {
        if (!await SetAuthHeader())
        {
            return default;
        }

        var response = await _http.PostAsJsonAsync(url, body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        return await ReadResponseAsync<T>(response);
    }

    public async Task<ApiResponse<T>?> PutAsync<T>(string url, object body)
    {
        if (!await SetAuthHeader())
        {
            return default;
        }

        var response = await _http.PutAsJsonAsync(url, body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        return await ReadResponseAsync<T>(response);
    }

    public async Task<ApiResponse<T>?> DeleteAsync<T>(string url)
    {
        if (!await SetAuthHeader())
        {
            return default;
        }

        var response = await _http.DeleteAsync(url);
        return await ReadResponseAsync<T>(response);
    }

    public async Task<ApiResponse<T>?> PostMultipartAsync<T>(string url, MultipartFormDataContent content)
    {
        if (!await SetAuthHeader())
        {
            return default;
        }

        var response = await _http.PostAsync(url, content);
        return await ReadResponseAsync<T>(response);
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
