using OmniFit.Application.DTOs.Workouts;
using OmniFit.Domain.Common;

namespace OmniFit.Application.Interfaces
{
    public interface IWorkoutService
    {
        Task<Guid> CreateWorkoutAsync(WorkoutRequest request, string userId);
        Task<PagedResult<WorkoutResponse>> GetAllWorkoutsAsync(WorkoutQueryParameters request);
        Task<PagedResult<WorkoutResponse>> GetWorkoutsByUserIdAsync(WorkoutQueryParameters request, string userId);
        Task<WorkoutResponse?> GetWorkoutByIdAsync(Guid id, string? userId = null);
        Task<bool> CompleteWorkoutAsync(Guid workoutId, string userId);
        Task<Guid?> AddExerciseAsync(Guid workoutId, WorkoutExerciseRequest request, string userId);
        Task<bool> RemoveExerciseAsync(Guid workoutId, Guid workoutExerciseId, string userId);
        Task<Guid?> AddSetAsync(Guid workoutId, Guid workoutExerciseId, WorkoutSetRequest request, string userId);
        Task<WorkoutSetResponse?> UpdateSetAsync(Guid workoutId, Guid workoutExerciseId, Guid workoutSetId, WorkoutSetRequest request, string userId);
        Task<bool> RemoveSetAsync(Guid workoutId, Guid workoutExerciseId, Guid workoutSetId, string userId);
    }
}
