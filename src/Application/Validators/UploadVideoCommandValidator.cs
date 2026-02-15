using Application.UseCases.UploadVideo;
using FluentValidation;

namespace Application.Validators;

public class UploadVideoCommandValidator : AbstractValidator<UploadVideoCommand>
{
    private static readonly string[] AllowedExtensions = { ".mp4", ".mov", ".avi", ".mkv" };
    private const long MaxFileSize = 100 * 1024 * 1024; // 100MB

    public UploadVideoCommandValidator()
    {
        RuleFor(x => x.VideoStream)
            .NotNull()
            .WithMessage("Video stream is required.");

        RuleFor(x => x.FileSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(MaxFileSize)
            .WithMessage($"Video file size must be between 1 byte and {MaxFileSize / (1024 * 1024)}MB.");

        RuleFor(x => x.FileName)
            .NotEmpty()
            .WithMessage("File name is required.")
            .Must(fileName => AllowedExtensions.Any(ext => fileName.ToLowerInvariant().EndsWith(ext)))
            .WithMessage($"Video file must be one of the following formats: {string.Join(", ", AllowedExtensions)}");
    }
}

