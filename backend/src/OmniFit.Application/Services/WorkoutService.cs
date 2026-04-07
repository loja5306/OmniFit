using OmniFit.Application.DTOs;
using OmniFit.Application.Interfaces;
using OmniFit.Application.Mapping;
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

        public async Task<IEnumerable<WorkoutResponse>> GetAllWorkoutsAsync()
        {
            var workouts = await _workoutRepository.GetAllAsync();

            return workouts.Select(w => w.MapToResponse());
        }

        public async Task<WorkoutResponse?> GetWorkoutByIdAsync(Guid id)
        {
            var workout = await _workoutRepository.GetByIdAsync(id);

            if (workout == null) return null;

            return workout.MapToResponse();
        }

        public async Task<IEnumerable<WorkoutResponse>> GetWorkoutsByUserIdAsync(string userId)
        {
            var workouts = await _workoutRepository.GetByUserIdAsync(userId);

            return workouts.Select(w => w.MapToResponse());
        }
    }
}