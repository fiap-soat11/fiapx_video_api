using Domain.Entities;

namespace Application.UseCases.GetVideoStatus;

public record GetVideoStatusResponse
{
    public Guid VideoId { get; init; }
    public VideoStatus Status { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}

