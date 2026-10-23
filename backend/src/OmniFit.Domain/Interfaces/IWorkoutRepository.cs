using OmniFit.Domain.Common;
using OmniFit.Domain.Entities;

namespace OmniFit.Domain.Interfaces
{
    public interface IWorkoutRepository
    {
        Task AddAsync(Workout workout);
        Task<Workout?> GetByIdAsync(Guid id, string? userId = null);
        Task<PagedResult<Workout>> GetAllAsync(int page, int pageSize);
        Task<PagedResult<Workout>> GetByUserIdAsync(int page, int pageSize, string userId);
        Task AddExerciseAsync(WorkoutExercise workoutExercise);
        void RemoveExercise(WorkoutExercise workoutExercise);
        Task AddSetAsync(WorkoutSet workoutSet);
        void UpdateSet(WorkoutSet workoutSet);
        void RemoveSet(WorkoutSet workoutSet);
        Task SaveChangesAsync();
    }
}
