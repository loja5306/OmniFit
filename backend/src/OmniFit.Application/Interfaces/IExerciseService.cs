using OmniFit.Application.DTOs;

namespace OmniFit.Application.Interfaces
{
    public interface IExerciseService
    {
        Task<Guid> CreateAsync(CreateExerciseRequest request);
        Task<IEnumerable<ExerciseResponse>> GetAllAsync();
        Task<ExerciseResponse?> GetByIdAsync(Guid id);
        Task<ExerciseResponse?> UpdateAsync(Guid id, UpdateExerciseRequest request);
        Task<bool> DeleteByIdAsync(Guid id);
    }
}
