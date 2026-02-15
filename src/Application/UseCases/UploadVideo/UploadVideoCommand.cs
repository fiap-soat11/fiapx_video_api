using MediatR;

namespace Application.UseCases.UploadVideo;

public sealed record UploadVideoCommand(Stream VideoStream, string FileName, string ContentType, long FileSize) : IRequest<UploadVideoResponse>;

