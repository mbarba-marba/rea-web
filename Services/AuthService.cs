using REA.Web.Models;

namespace REA.Web.Services;

public class AuthService
{
    private readonly ApiClient _api;
    private readonly TokenService _tokenService;

    public AuthService(ApiClient api, TokenService tokenService)
    {
        _api = api;
        _tokenService = tokenService;
    }

    public async Task<ApiResponse<AuthResponse>?> LoginAsync(string email, string password)
    {
        var result = await _api.PostUnauthAsync<AuthResponse>("api/v1/auth/login", new LoginRequest { Email = email, Password = password });
        if (result?.Data != null && !result.Data.RequiereMfa)
        {
            await _tokenService.SaveTokensAsync(result.Data.AccessToken, result.Data.RefreshToken);
        }
        return result;
    }

    public async Task<ApiResponse<AuthResponse>?> VerifyMfaAsync(string mfaTokenTemp, string codigoTotp)
    {
        var result = await _api.PostUnauthAsync<AuthResponse>("api/v1/auth/mfa/verify", new MfaVerifyRequest { MfaTokenTemp = mfaTokenTemp, CodigoTotp = codigoTotp });
        if (result?.Data != null)
        {
            await _tokenService.SaveTokensAsync(result.Data.AccessToken, result.Data.RefreshToken);
        }
        return result;
    }

    public async Task<ApiResponse<MfaSetupResponse>?> SetupMfaAsync()
    {
        return await _api.PostAsync<MfaSetupResponse>("api/v1/auth/mfa/setup", new { });
    }

    public async Task<ApiResponse<MfaConfirmResponse>?> ConfirmMfaAsync(string codigoTotp)
    {
        return await _api.PostAsync<MfaConfirmResponse>("api/v1/auth/mfa/confirm", new { codigoTotp = codigoTotp });
    }

    public async Task LogoutAsync()
    {
        var refreshToken = await _tokenService.GetRefreshTokenAsync();
        if (!string.IsNullOrEmpty(refreshToken))
        {
            await _api.PostUnauthAsync("api/v1/auth/logout", new RefreshRequest { RefreshToken = refreshToken });
        }
        await _tokenService.ClearTokensAsync();
    }

    public async Task<ApiResponse<AuthResponse>?> RefreshAsync()
    {
        var refreshToken = await _tokenService.GetRefreshTokenAsync();
        if (string.IsNullOrEmpty(refreshToken)) return null;

        var result = await _api.PostUnauthAsync<AuthResponse>("api/v1/auth/refresh", new RefreshRequest { RefreshToken = refreshToken });
        if (result?.Data != null)
        {
            await _tokenService.SaveTokensAsync(result.Data.AccessToken, result.Data.RefreshToken);
        }
        return result;
    }
}
