using Microsoft.AspNetCore.Http;

namespace WebApi.Models;

public class UploadVideoRequest
{
    public int UserId { get; set; }
    public IFormFile VideoFile { get; set; } = null!;
}

