namespace OmniFit.Application.DTOs.Workouts
{
    public record WorkoutSetResponse(
        Guid Id,
        Guid WorkoutExerciseId,
        int SetNumber,
        int Reps,
        double Weight
    );
}
