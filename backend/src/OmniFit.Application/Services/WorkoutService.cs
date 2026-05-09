using OmniFit.Application.DTOs.Workouts;
using OmniFit.Application.Interfaces;
using OmniFit.Application.Mapping;
using OmniFit.Domain.Common;
using OmniFit.Domain.Interfaces;

namespace OmniFit.Application.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly IWorkoutRepository _workoutRepository;

        public WorkoutService(IWorkoutRepository workoutRepository)
        {
            _workoutRepository = workoutRepository;
        }

        public async Task<Guid> CreateWorkoutAsync(CreateWorkoutRequest request, string userId)
        {
            var workout = request.MapToEntity(userId);

            await _workoutRepository.AddAsync(workout);
            await _workoutRepository.SaveChangesAsync();

            return workout.Id;
        }

        public async Task<PagedResult<WorkoutResponse>> GetAllWorkoutsAsync(WorkoutQueryParameters request)
        {
            var workouts = await _workoutRepository.GetAllAsync(request.Page, request.PageSize);

            return workouts.MapToResponse();
        }

        public async Task<WorkoutResponse?> GetWorkoutByIdAsync(Guid id)
        {
            var workout = await _workoutRepository.GetByIdAsync(id);

            if (workout == null) return null;

            return workout.MapToResponse();
        }

        public async Task<PagedResult<WorkoutResponse>> GetWorkoutsByUserIdAsync(WorkoutQueryParameters request, string userId)
        {
            var workouts = await _workoutRepository.GetByUserIdAsync(request.Page, request.PageSize, userId);

            return workouts.MapToResponse();
        }
    }
}