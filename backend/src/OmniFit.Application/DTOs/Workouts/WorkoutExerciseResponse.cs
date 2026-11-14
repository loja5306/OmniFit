namespace OmniFit.Application.DTOs.Workouts
{
    public record WorkoutExerciseResponse(
        Guid Id,
        Guid ExerciseId,
        string Name,
        IEnumerable<WorkoutSetResponse> WorkoutSets
    );
}