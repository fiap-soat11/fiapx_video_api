using MediatR;

namespace Application.UseCases.GetProcessedVideo;

public sealed record GetProcessedVideoQuery(int Id) : IRequest<GetProcessedVideoResponse>;

