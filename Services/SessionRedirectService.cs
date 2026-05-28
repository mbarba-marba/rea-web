using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using REA.Web.Providers;

namespace REA.Web.Services;

public class SessionRedirectService
{
    private readonly TokenService _tokenService;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly NavigationManager _navigation;

    public SessionRedirectService(
        TokenService tokenService,
        AuthenticationStateProvider authStateProvider,
        NavigationManager navigation)
    {
        _tokenService = tokenService;
        _authStateProvider = authStateProvider;
        _navigation = navigation;
    }

    public async Task RedirectToLoginAsync()
    {
        await _tokenService.ClearTokensAsync();

        if (_authStateProvider is ReaAuthStateProvider authProvider)
        {
            await authProvider.MarkUserAsLoggedOut();
        }

        var absolute = _navigation.ToAbsoluteUri(_navigation.Uri);
        if (!absolute.AbsolutePath.Equals("/login", StringComparison.OrdinalIgnoreCase))
        {
            _navigation.NavigateTo("/login", forceLoad: false);
        }
    }
}
