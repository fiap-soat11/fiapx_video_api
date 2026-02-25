using Application.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class VideoRepository(ApplicationDbContext context) : IVideoRepository
{
    public async Task<VideoProcessing?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.VideoProcessings
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }

    public async Task<VideoProcessing> AddAsync(VideoProcessing videoProcessing, CancellationToken cancellationToken = default)
    {
        await context.VideoProcessings.AddAsync(videoProcessing, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return videoProcessing;
    }

    public async Task UpdateAsync(VideoProcessing videoProcessing, CancellationToken cancellationToken = default)
    {
        context.VideoProcessings.Update(videoProcessing);
        await context.SaveChangesAsync(cancellationToken);
    }
}

