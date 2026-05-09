using OmniFit.Application.DTOs.Workouts;
using OmniFit.Domain.Common;
using OmniFit.Domain.Entities;

namespace OmniFit.Application.Mapping
{
    public static class WorkoutMappingExtensions
    {
        public static Workout MapToEntity(this CreateWorkoutRequest request, string userId)
        {
            return new Workout
            {
                Name = request.Name,
                UserId = userId,
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
        }

        public static WorkoutResponse MapToResponse(this Workout workout)
        {
            return new WorkoutResponse(
                workout.Id, 
                workout.Name, 
                workout.WorkoutExercises.Count());
        }

        public static PagedResult<WorkoutResponse> MapToResponse(
            this PagedResult<Workout> workouts)
        {
            return new PagedResult<WorkoutResponse>(
                workouts.Items.Select(w => w.MapToResponse()),
                workouts.Page,
                workouts.PageSize,
                workouts.TotalCount
            );
        }
    }
}
