using OmniFit.Application.DTOs.Exercises;
using OmniFit.Domain.Common;

namespace OmniFit.Application.Interfaces
{
    public interface IExerciseService
    {
        Task<Guid> CreateAsync(CreateExerciseRequest request);
        Task<PagedResult<ExerciseResponse>> GetAllAsync(ExerciseQueryParameters request);
        Task<ExerciseResponse?> GetByIdAsync(Guid id);
        Task<ExerciseResponse?> UpdateAsync(Guid id, UpdateExerciseRequest request);
        Task<bool> DeleteByIdAsync(Guid id);
    }
}
