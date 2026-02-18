namespace WebApi.Models;

public record HealthCheckResponse
{
    public string Status { get; init; } = string.Empty;
    public DateTime Timestamp { get; init; }
}

