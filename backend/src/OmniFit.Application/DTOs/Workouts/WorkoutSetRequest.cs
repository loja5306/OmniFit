namespace OmniFit.Application.DTOs.Workouts
{
    public record WorkoutSetRequest(
        int SetNumber,
        int Reps,
        int Weight
    );
}
