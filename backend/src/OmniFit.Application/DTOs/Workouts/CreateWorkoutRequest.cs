namespace OmniFit.Application.DTOs.Workouts
{
    public record CreateWorkoutRequest(
        string Name,
        List<WorkoutExerciseRequest>? Exercises = null
    );
}
