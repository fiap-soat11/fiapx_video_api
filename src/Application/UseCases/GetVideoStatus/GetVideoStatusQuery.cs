using MediatR;

namespace Application.UseCases.GetVideoStatus;

public sealed record GetVideoStatusQuery(int Id) : IRequest<GetVideoStatusResponse>;

