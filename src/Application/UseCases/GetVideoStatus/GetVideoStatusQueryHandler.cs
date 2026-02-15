using Application.Repositories;
using MediatR;

namespace Application.UseCases.GetVideoStatus;

public sealed class GetVideoStatusQueryHandler : IRequestHandler<GetVideoStatusQuery, GetVideoStatusResponse>
{
    private readonly IVideoRepository _videoRepository;

    public GetVideoStatusQueryHandler(IVideoRepository videoRepository)
    {
        _videoRepository = videoRepository;
    }

    public async Task<GetVideoStatusResponse> Handle(GetVideoStatusQuery request, CancellationToken cancellationToken)
    {
        var video = await _videoRepository.GetByIdAsync(request.VideoId, cancellationToken);

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

