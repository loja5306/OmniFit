using OmniFit.Application.DTOs;
using OmniFit.Application.Interfaces;
using OmniFit.Domain.Entities;
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

        public async Task<Guid> CreateWorkoutAsync(CreateWorkoutRequest request)
        {
            var workout = new Workout
            {
                Name = request.Name,
                WorkoutExercises = request.Exercises?.Select(e => new WorkoutExercise
                {
                    ExerciseId = e.ExerciseId,
                    WorkoutSets = e.Sets?.Select(s => new WorkoutSet
                    {
                        SetNumber = s.SetNumber,
                        Reps = s.Reps,
                        Weight = s.Weight
                    }).ToList() ?? new()
                }).ToList() ?? new()
            };

            await _workoutRepository.AddAsync(workout);
            await _workoutRepository.SaveChangesAsync();

            return workout.Id;
        }

        public async Task<IEnumerable<WorkoutResponse>> GetAllWorkoutsAsync()
        {
            var workouts = await _workoutRepository.GetAllAsync();

            return workouts.Select(w => new WorkoutResponse(
                w.Id,
                w.Name,
                w.WorkoutExercises.Count()));
        }

        public async Task<WorkoutResponse?> GetWorkoutByIdAsync(Guid id)
        {
            var workout = await _workoutRepository.GetByIdAsync(id);

            if (workout == null) return null;

            return new WorkoutResponse(
                workout.Id,
                workout.Name,
                workout.WorkoutExercises.Count());
        }
    }
}
