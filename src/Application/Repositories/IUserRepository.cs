namespace Application.Repositories;

public interface IUserRepository
{
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
