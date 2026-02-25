namespace Application.UseCases.UploadVideo;

public record UploadVideoResponse
{
    public int Id { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}
