using OmniFit.Application.DTOs;
using OmniFit.Domain.Entities;

namespace OmniFit.Application.Mapping
{
    public static class WorkoutMappingExtensions
    {
        public static Workout MapToEntity(this CreateWorkoutRequest request)
        {
            return new Workout
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
        }

        public static WorkoutResponse MapToResponse(this Workout workout)
        {
            return new WorkoutResponse(
                workout.Id, 
                workout.Name, 
                workout.WorkoutExercises.Count());
        }
    }
}
