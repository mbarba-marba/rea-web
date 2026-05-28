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

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

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

        return await ReadApiResponseAsync<T>(response);
    }

    private static async Task<ApiResponse<T>?> ReadApiResponseAsync<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content))
        {
            return new ApiResponse<T>
            {
                Ok = response.IsSuccessStatusCode,
                Error = response.IsSuccessStatusCode
                    ? null
                    : new ApiError
                    {
                        Code = $"HTTP_{(int)response.StatusCode}",
                        Message = "La respuesta del servidor llego vacia."
                    }
            };
        }

        try
        {
            return JsonSerializer.Deserialize<ApiResponse<T>>(content, JsonOptions);
        }
        catch (JsonException)
        {
            return new ApiResponse<T>
            {
                Ok = false,
                Error = new ApiError
                {
                    Code = $"HTTP_{(int)response.StatusCode}",
                    Message = ExtraerMensajePlano(content)
                }
            };
        }
    }

    private static string ExtraerMensajePlano(string content)
    {
        var compact = content.Trim();
        if (compact.StartsWith("<", StringComparison.Ordinal))
        {
            return "El servidor devolvio una respuesta invalida. Intenta nuevamente en unos segundos.";
        }

        return compact.Length > 240
            ? compact[..240] + "..."
            : compact;
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

        var response = await _http.PostAsJsonAsync(url, body, JsonOptions);
        return await ReadResponseAsync<T>(response);
    }

    public async Task<ApiResponse<T>?> PutAsync<T>(string url, object body)
    {
        if (!await SetAuthHeader())
        {
            return default;
        }

        var response = await _http.PutAsJsonAsync(url, body, JsonOptions);
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
        await _http.PostAsJsonAsync(url, body, JsonOptions);
    }

    public async Task<ApiResponse<T>?> PostUnauthAsync<T>(string url, object body)
    {
        _http.DefaultRequestHeaders.Authorization = null;
        var response = await _http.PostAsJsonAsync(url, body, JsonOptions);
        return await ReadApiResponseAsync<T>(response);
    }
}
