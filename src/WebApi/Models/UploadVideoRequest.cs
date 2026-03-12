using Microsoft.AspNetCore.Http;

namespace WebApi.Models;

public class UploadVideoRequest
{
    public IFormFile VideoFile { get; set; } = null!;
}

