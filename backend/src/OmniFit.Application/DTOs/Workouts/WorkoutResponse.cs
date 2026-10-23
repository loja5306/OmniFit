using OmniFit.Domain.Enums;

namespace OmniFit.Application.DTOs.Workouts
{
    public record WorkoutResponse(
        Guid Id,
        string Name,
        WorkoutStatus Status,
        DateTimeOffset? CompletedOn,
        int TotalExercises
    );
}
