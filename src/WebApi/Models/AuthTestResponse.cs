namespace WebApi.Models;

public record AuthTestResponse
{
    public bool IsAuthenticated { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public object? Claims { get; init; }
}

