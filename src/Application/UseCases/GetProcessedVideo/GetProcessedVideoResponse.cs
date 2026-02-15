using Domain.Entities;

namespace Application.UseCases.GetProcessedVideo;

public record GetProcessedVideoResponse
{
    public Guid VideoId { get; init; }
    public VideoStatus Status { get; init; }
    public string? DownloadUrl { get; init; }
    public DateTime? ExpiresAt { get; init; }
}

