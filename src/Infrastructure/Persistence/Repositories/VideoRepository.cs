using Application.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class VideoRepository(ApplicationDbContext context) : IVideoRepository
{
    public async Task<Video?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Videos
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }

    public async Task<Video> AddAsync(Video video, CancellationToken cancellationToken = default)
    {
        await context.Videos.AddAsync(video, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return video;
    }

    public async Task UpdateAsync(Video video, CancellationToken cancellationToken = default)
    {
        context.Videos.Update(video);
        await context.SaveChangesAsync(cancellationToken);
    }
}

