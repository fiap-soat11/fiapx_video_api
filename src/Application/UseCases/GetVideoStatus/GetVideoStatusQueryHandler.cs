using Application.Repositories;
using MediatR;

namespace Application.UseCases.GetVideoStatus;

public sealed class GetVideoStatusQueryHandler(IVideoRepository videoRepository) : IRequestHandler<GetVideoStatusQuery, GetVideoStatusResponse>
{
    public async Task<GetVideoStatusResponse> Handle(GetVideoStatusQuery request, CancellationToken cancellationToken)
    {
        var video = await videoRepository.GetByIdAsync(request.VideoId, cancellationToken);

        if (video == null)
        {
            throw new KeyNotFoundException($"Video with id {request.VideoId} not found.");
        }

        return new GetVideoStatusResponse
        {
            VideoId = video.Id,
            Status = video.Status,
            CreatedAt = video.CreatedAt,
            UpdatedAt = video.UpdatedAt
        };
    }
}

