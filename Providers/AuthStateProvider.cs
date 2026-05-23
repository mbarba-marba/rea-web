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

        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);
        return new AuthenticationState(user);
    }

    public async Task MarkUserAsAuthenticated(string accessToken)
    {
        await _storage.SetAsync("rea_access_token", accessToken);
        var claims = ParseClaimsFromJwt(accessToken);
        var identity = new ClaimsIdentity(claims, "jwt");
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

            if (doc.RootElement.TryGetProperty(ClaimTypes.NameIdentifier.Replace("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/", ""), out var nameId))
                claims.Add(new Claim(ClaimTypes.NameIdentifier, nameId.GetString()!));
            if (doc.RootElement.TryGetProperty(ClaimTypes.Email.Replace("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/", ""), out var email))
                claims.Add(new Claim(ClaimTypes.Email, email.GetString()!));
            if (doc.RootElement.TryGetProperty(ClaimTypes.Role.Replace("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/", ""), out var role))
                claims.Add(new Claim(ClaimTypes.Role, role.GetString()!));
            if (doc.RootElement.TryGetProperty(ClaimTypes.Name.Replace("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/", ""), out var name))
                claims.Add(new Claim(ClaimTypes.Name, name.GetString()!));

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
