using OmniFit.Application.DTOs.Workouts;
using OmniFit.Application.Interfaces;
using OmniFit.Application.Mapping;
using OmniFit.Domain.Common;
using OmniFit.Domain.Entities;
using OmniFit.Domain.Enums;
using OmniFit.Domain.Exceptions;
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

        private void ThrowIfWorkoutCompleted(Workout workout) 
        {
            if (workout.Status == WorkoutStatus.Completed)
            {
                throw new BusinessRuleException("Status", "Cannot modify a completed workout");
            }
        }

        public async Task<Guid> CreateWorkoutAsync(WorkoutRequest request, string userId)
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

        public async Task<PagedResult<WorkoutResponse>> GetWorkoutsByUserIdAsync(WorkoutQueryParameters request, string userId)
        {
            var workouts = await _workoutRepository.GetByUserIdAsync(request.Page, request.PageSize, userId);

            return workouts.MapToResponse();
        }

        public async Task<WorkoutResponse?> GetWorkoutByIdAsync(Guid id, string? userId = null)
        {
            var workout = await _workoutRepository.GetByIdAsync(id, userId);

            if (workout == null) return null;

            return workout.MapToResponse();
        }

        public async Task<bool> CompleteWorkoutAsync(Guid workoutId, string userId)
        {
            var workout = await _workoutRepository.GetByIdAsync(workoutId, userId);
            if (workout == null) return false;

            ThrowIfWorkoutCompleted(workout);
            if (workout.WorkoutExercises.Count == 0) throw new BusinessRuleException("WorkoutExercises", "A workout must have at least 1 exercise");
            if (workout.WorkoutExercises.Any(we => we.WorkoutSets.Count == 0)) throw new BusinessRuleException("WorkoutExercises", "Each exercise must have at least 1 set");

            workout.Status = WorkoutStatus.Completed;
            workout.CompletedOn = DateTimeOffset.UtcNow;
            await _workoutRepository.SaveChangesAsync();
            return true;
        }

        public async Task<Guid?> AddExerciseAsync(Guid workoutId, WorkoutExerciseRequest request, string userId)
        {
            var workout = await _workoutRepository.GetByIdAsync(workoutId, userId);
            if (workout == null) return null;

            ThrowIfWorkoutCompleted(workout);

            var workoutExercise = new WorkoutExercise
            {
                WorkoutId = workoutId,
                ExerciseId = request.ExerciseId
            };

            await _workoutRepository.AddExerciseAsync(workoutExercise);
            await _workoutRepository.SaveChangesAsync();
            return workoutExercise.Id;
        }

        public async Task<bool> RemoveExerciseAsync(Guid workoutId, Guid workoutExerciseId, string userId)
        {
            var workout = await _workoutRepository.GetByIdAsync(workoutId, userId);
            if (workout == null) return false;

            ThrowIfWorkoutCompleted(workout);

            var workoutExercise = workout.WorkoutExercises.FirstOrDefault(we => we.Id == workoutExerciseId);
            if (workoutExercise == null) return false;

            _workoutRepository.RemoveExercise(workoutExercise);
            await _workoutRepository.SaveChangesAsync();

            return true;
        }

        public async Task<Guid?> AddSetAsync(Guid workoutId, Guid workoutExerciseId, WorkoutSetRequest request, string userId)
        {
            var workout = await _workoutRepository.GetByIdAsync(workoutId, userId);
            if (workout == null) return null;

            ThrowIfWorkoutCompleted(workout);

            if (!workout.WorkoutExercises.Any(we => we.Id == workoutExerciseId)) return null;

            var workoutSet = new WorkoutSet
            {
                WorkoutExerciseId = workoutExerciseId,
                SetNumber = request.SetNumber,
                Reps = request.Reps,
                Weight = request.Weight
            };

            await _workoutRepository.AddSetAsync(workoutSet);
            await _workoutRepository.SaveChangesAsync();
            return workoutSet.Id;
        }

        public async Task<WorkoutSetResponse?> UpdateSetAsync(Guid workoutId, Guid workoutExerciseId, Guid workoutSetId, WorkoutSetRequest request, string userId)
        {
            var workout = await _workoutRepository.GetByIdAsync(workoutId, userId);
            if (workout == null) return null;

            ThrowIfWorkoutCompleted(workout);

            var workoutExercise = workout.WorkoutExercises.FirstOrDefault(we => we.Id == workoutExerciseId);
            if (workoutExercise == null) return null;

            var workoutSet = workoutExercise.WorkoutSets.FirstOrDefault(ws => ws.Id == workoutSetId);
            if (workoutSet == null) return null;

            workoutSet.SetNumber = request.SetNumber;
            workoutSet.Reps = request.Reps;
            workoutSet.Weight = request.Weight;

            _workoutRepository.UpdateSet(workoutSet);
            await _workoutRepository.SaveChangesAsync();

            return workoutSet.MapToResponse();
        }
        

        public async Task<bool> RemoveSetAsync(Guid workoutId, Guid workoutExerciseId, Guid workoutSetId, string userId)
        {
            var workout = await _workoutRepository.GetByIdAsync(workoutId, userId);
            if (workout == null) return false;

            ThrowIfWorkoutCompleted(workout);

            var workoutExercise = workout.WorkoutExercises.FirstOrDefault(we => we.Id == workoutExerciseId);
            if (workoutExercise == null) return false;

            var workoutSet = workoutExercise.WorkoutSets.FirstOrDefault(ws => ws.Id == workoutSetId);
            if (workoutSet == null) return false;

            _workoutRepository.RemoveSet(workoutSet);
            await _workoutRepository.SaveChangesAsync();

            return true;
        }
    }
}