namespace OmniFit.Application.DTOs.Workouts
{
    public record WorkoutExerciseRequest(
        Guid ExerciseId,
        List<WorkoutSetRequest>? Sets = null
    );
}
