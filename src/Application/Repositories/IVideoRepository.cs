using Domain.Entities;

namespace Application.Repositories;

public interface IVideoRepository
{
    Task<VideoProcessing?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<VideoProcessing> AddAsync(VideoProcessing videoProcessing, CancellationToken cancellationToken = default);
    Task UpdateAsync(VideoProcessing videoProcessing, CancellationToken cancellationToken = default);
}

