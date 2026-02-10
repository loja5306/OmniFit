using OmniFit.Application.DTOs;

namespace OmniFit.Application.Interfaces
{
    public interface IWorkoutService
    {
        Task<Guid> CreateWorkoutAsync(CreateWorkoutRequest request, string userId);
        Task<IEnumerable<WorkoutResponse>> GetAllWorkoutsAsync();
        Task<IEnumerable<WorkoutResponse>> GetWorkoutsByUserIdAsync(string userId);
        Task<WorkoutResponse?> GetWorkoutByIdAsync(Guid id);
    }
}
