using OmniFit.Application.DTOs.Workouts;
using OmniFit.Domain.Common;

namespace OmniFit.Application.Interfaces
{
    public interface IWorkoutService
    {
        Task<Guid> CreateWorkoutAsync(CreateWorkoutRequest request, string userId);
        Task<PagedResult<WorkoutResponse>> GetAllWorkoutsAsync(WorkoutQueryParameters request);
        Task<PagedResult<WorkoutResponse>> GetWorkoutsByUserIdAsync(WorkoutQueryParameters request, string userId);
        Task<WorkoutResponse?> GetWorkoutByIdAsync(Guid id);
    }
}
