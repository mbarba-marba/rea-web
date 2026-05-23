namespace REA.Web.Models;

public class ApiResponse<T>
{
    public bool Ok { get; set; }
    public T? Data { get; set; }
    public ApiMeta? Meta { get; set; }
    public ApiError? Error { get; set; }
}

public class ApiMeta
{
    public int Page { get; set; }
    public int Limit { get; set; }
    public int Total { get; set; }
}

public class ApiError
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public object? Details { get; set; }
}
