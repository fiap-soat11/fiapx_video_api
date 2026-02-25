namespace Application.UseCases.GetVideoStatus;

public record GetVideoStatusResponse
{
    public int Id { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime? CompletedAt { get; init; }
}
