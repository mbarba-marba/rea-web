using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace REA.Web.Providers;

public class ReaAuthStateProvider : AuthenticationStateProvider
{
    private readonly Services.ILocalStorageService _storage;
    private static readonly AuthenticationState Anonymous = new(new ClaimsPrincipal(new ClaimsIdentity()));

    public ReaAuthStateProvider(Services.ILocalStorageService storage)
    {
        _storage = storage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _storage.GetAsync("rea_access_token");
        if (string.IsNullOrEmpty(token))
            return Anonymous;

        try
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var identity = new ClaimsIdentity(jwt.Claims, "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        catch
        {
            return Anonymous;
        }
    }

    public async Task MarkUserAsAuthenticated(string accessToken)
    {
        await _storage.SetAsync("rea_access_token", accessToken);
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
        var identity = new ClaimsIdentity(jwt.Claims, "jwt");
        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _storage.RemoveAsync("rea_access_token");
        NotifyAuthenticationStateChanged(Task.FromResult(Anonymous));
    }
}
