namespace REA.Web.Services;

public class AuthDelegatingHandler : DelegatingHandler
{
    private readonly TokenService _tokenService;
    private readonly SessionRedirectService _sessionRedirect;

    public AuthDelegatingHandler(TokenService tokenService, SessionRedirectService sessionRedirect)
    {
        _tokenService = tokenService;
        _sessionRedirect = sessionRedirect;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _tokenService.GetAccessTokenAsync();
        if (_tokenService.IsTokenExpired(token))
        {
            await _sessionRedirect.RedirectToLoginAsync();
            return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
            {
                RequestMessage = request
            };
        }

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        var response = await base.SendAsync(request, cancellationToken);
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            await _sessionRedirect.RedirectToLoginAsync();
        }

        return response;
    }
}
