namespace REA.Web.Models;

public class AuthResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public bool RequiereMfa { get; set; }
    public string? MfaTokenTemp { get; set; }
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class MfaVerifyRequest
{
    public string MfaTokenTemp { get; set; } = string.Empty;
    public string CodigoTotp { get; set; } = string.Empty;
}

public class MfaSetupResponse
{
    public string SecretoTotp { get; set; } = string.Empty;
    public string QrUri { get; set; } = string.Empty;
}

public class MfaConfirmResponse
{
    public List<string> CodigosRespaldo { get; set; } = new();
}

public class RefreshRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}
