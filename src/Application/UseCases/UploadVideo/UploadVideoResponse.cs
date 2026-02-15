using Domain.Entities;

namespace Application.UseCases.UploadVideo;

public record UploadVideoResponse
{
    public Guid VideoId { get; init; }
    public VideoStatus Status { get; init; }
    public DateTime UploadedAt { get; init; }
}

