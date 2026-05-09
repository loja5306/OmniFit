using OmniFit.Domain.Common;
using OmniFit.Domain.Entities;

namespace OmniFit.Domain.Interfaces
{
    public interface IWorkoutRepository
    {
        Task AddAsync(Workout workout);
        Task<Workout?> GetByIdAsync(Guid id);
        Task<PagedResult<Workout>> GetAllAsync(int page, int pageSize);
        Task<PagedResult<Workout>> GetByUserIdAsync(int page, int pageSize, string userId);
        Task SaveChangesAsync();
    }
}
