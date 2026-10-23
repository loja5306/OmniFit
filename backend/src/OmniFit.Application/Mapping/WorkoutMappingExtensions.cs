using OmniFit.Application.DTOs.Workouts;
using OmniFit.Domain.Common;
using OmniFit.Domain.Entities;
using OmniFit.Domain.Enums;

namespace OmniFit.Application.Mapping
{
    public static class WorkoutMappingExtensions
    {
        public static Workout MapToEntity(this WorkoutRequest request, string userId)
        {
            return new Workout
            {
                Name = request.Name,
                UserId = userId,
                Status = WorkoutStatus.InProgress
            };
        }

        public static WorkoutResponse MapToResponse(this Workout workout)
        {
            return new WorkoutResponse(
                workout.Id, 
                workout.Name,
                workout.Status,
                workout.CompletedOn,
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

        public static WorkoutSetResponse MapToResponse(this WorkoutSet workoutSet)
        {
            return new WorkoutSetResponse(
                workoutSet.Id, 
                workoutSet.WorkoutExerciseId, 
                workoutSet.SetNumber, 
                workoutSet.Reps, 
                workoutSet.Weight
            );
        }
    }
}
