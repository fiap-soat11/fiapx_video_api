using MediatR;

namespace Application.UseCases.GetProcessedVideo;

public sealed record GetProcessedVideoQuery(Guid VideoId) : IRequest<GetProcessedVideoResponse>;

