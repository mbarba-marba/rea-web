using System.Security.Claims;
using System.Text.Json;
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

        var claims = ParseClaimsFromJwt(token);
        if (claims.Length == 0)
            return Anonymous;

        var identity = new ClaimsIdentity(claims, "jwt", ClaimsIdentity.DefaultNameClaimType, ClaimTypes.Role);
        var user = new ClaimsPrincipal(identity);
        return new AuthenticationState(user);
    }

    public async Task MarkUserAsAuthenticated(string accessToken)
    {
        await _storage.SetAsync("rea_access_token", accessToken);
        var claims = ParseClaimsFromJwt(accessToken);
        var identity = new ClaimsIdentity(claims, "jwt", ClaimsIdentity.DefaultNameClaimType, ClaimTypes.Role);
        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _storage.RemoveAsync("rea_access_token");
        NotifyAuthenticationStateChanged(Task.FromResult(Anonymous));
    }

    private static Claim[] ParseClaimsFromJwt(string jwt)
    {
        try
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var json = System.Text.Encoding.UTF8.GetString(jsonBytes);

            using var doc = JsonDocument.Parse(json);
            var claims = new List<Claim>();

            foreach (var prop in doc.RootElement.EnumerateObject())
            {
                var value = prop.Value.GetString();
                if (string.IsNullOrEmpty(value)) continue;

                var claimType = prop.Name switch
                {
                    "sub" or "nameid" => ClaimTypes.NameIdentifier,
                    "email" or "emailaddress" => ClaimTypes.Email,
                    "role" or "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" => ClaimTypes.Role,
                    "name" or "unique_name" => ClaimTypes.Name,
                    _ => prop.Name
                };

                claims.Add(new Claim(claimType, value));
            }

            return claims.ToArray();
        }
        catch
        {
            return Array.Empty<Claim>();
        }
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}
