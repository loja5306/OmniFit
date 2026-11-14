using System.Text.Json.Serialization;
using OmniFit.Domain.Enums;

namespace OmniFit.Application.DTOs.Workouts
{
    public record WorkoutSummaryResponse(
        Guid Id,
        string Name,
        [property: JsonConverter(typeof(JsonStringEnumConverter))]
        WorkoutStatus Status,
        DateTimeOffset? CompletedOn,
        int TotalExercises
    );
}
