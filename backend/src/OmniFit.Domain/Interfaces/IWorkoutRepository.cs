using OmniFit.Domain.Entities;

namespace OmniFit.Domain.Interfaces
{
    public interface IWorkoutRepository
    {
        Task AddAsync(Workout workout);
        Task<Workout?> GetByIdAsync(Guid id);
        Task<IEnumerable<Workout>> GetAllAsync();
        Task SaveChangesAsync();
    }
}
