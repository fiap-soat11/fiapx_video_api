using Application.Repositories;
using MediatR;

namespace Application.UseCases.GetVideoStatus;

public sealed class GetVideoStatusQueryHandler(IVideoRepository videoRepository) : IRequestHandler<GetVideoStatusQuery, GetVideoStatusResponse>
{
    public async Task<GetVideoStatusResponse> Handle(GetVideoStatusQuery request, CancellationToken cancellationToken)
    {
        var video = await videoRepository.GetByIdAsync(request.Id, cancellationToken);

        if (video == null)
        {
            throw new KeyNotFoundException($"Video with id {request.Id} not found.");
        }

        return new GetVideoStatusResponse
        {
            Id = video.Id,
            Status = video.Status,
            CreatedAt = video.CreatedAt,
            CompletedAt = video.CompletedAt
        };
    }
}
