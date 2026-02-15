using Application.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class VideoRepository : IVideoRepository
{
    private readonly ApplicationDbContext _context;

    public VideoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Video?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Videos
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }

    public async Task<Video> AddAsync(Video video, CancellationToken cancellationToken = default)
    {
        await _context.Videos.AddAsync(video, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return video;
    }

    public async Task UpdateAsync(Video video, CancellationToken cancellationToken = default)
    {
        _context.Videos.Update(video);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

