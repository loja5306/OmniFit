namespace OmniFit.Application.DTOs.Workouts
{
    public record WorkoutResponse(
        Guid Id,
        string Name,
        int TotalExercises
    );
}
