using MediatR;

namespace Application.UseCases.GetVideoStatus;

public sealed record GetVideoStatusQuery(Guid VideoId) : IRequest<GetVideoStatusResponse>;

