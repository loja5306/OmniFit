using OmniFit.Domain.Common;
using OmniFit.Domain.Entities;

namespace OmniFit.Domain.Interfaces
{
    public interface IExerciseRepository
    {
        Task AddAsync(Exercise exercise);
        Task<Exercise?> GetByIdAsync(Guid id);
        Task<PagedResult<Exercise>> GetAllAsync(int page, int pageSize);
        void Update(Exercise exercise);
        void Delete(Exercise exercise);
        Task SaveChangesAsync();
    }
}
