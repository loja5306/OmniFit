using OmniFit.Application.DTOs;

namespace OmniFit.Application.Interfaces
{
    public interface IWorkoutService
    {
        Task<Guid> CreateWorkoutAsync(CreateWorkoutRequest request);
        Task<IEnumerable<WorkoutResponse>> GetAllWorkoutsAsync();
        Task<WorkoutResponse?> GetWorkoutByIdAsync(Guid id);
    }
}
