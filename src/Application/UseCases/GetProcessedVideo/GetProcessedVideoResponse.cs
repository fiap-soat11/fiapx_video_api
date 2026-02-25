namespace Application.UseCases.GetProcessedVideo;

public record GetProcessedVideoResponse
{
    public int Id { get; init; }
    public string Status { get; init; } = string.Empty;
    public string? DownloadUrl { get; init; }
    public DateTime? ExpiresAt { get; init; }
    /// <summary>Quando preenchido, a API deve retornar este stream como arquivo (FileResult).</summary>
    public Stream? FileStream { get; init; }
    public string? FileName { get; init; }
    public string? ContentType { get; init; }
}
