namespace OmniFit.Application.DTOs
{
    public record CreateWorkoutRequest(
        string Name,
        List<WorkoutExerciseRequest>? Exercises = null
    );

    public record WorkoutExerciseRequest(
        Guid ExerciseId,
        List<WorkoutSetRequest>? Sets = null
    );

    public record WorkoutSetRequest(
        int SetNumber,
        int Reps,
        int Weight
    );

    public record WorkoutResponse(
        Guid Id,
        string Name,
        int TotalExercises
    );
}
