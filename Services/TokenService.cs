namespace REA.Web.Services;

public class TokenService
{
    private const string AccessTokenKey = "rea_access_token";
    private const string RefreshTokenKey = "rea_refresh_token";
    private readonly ILocalStorageService _storage;

    public TokenService(ILocalStorageService storage)
    {
        _storage = storage;
    }

    public async Task<string?> GetAccessTokenAsync() => await _storage.GetAsync(AccessTokenKey);
    public async Task<string?> GetRefreshTokenAsync() => await _storage.GetAsync(RefreshTokenKey);
    public async Task SaveTokensAsync(string accessToken, string refreshToken)
    {
        await _storage.SetAsync(AccessTokenKey, accessToken);
        await _storage.SetAsync(RefreshTokenKey, refreshToken);
    }
    public async Task ClearTokensAsync()
    {
        await _storage.RemoveAsync(AccessTokenKey);
        await _storage.RemoveAsync(RefreshTokenKey);
    }
}
