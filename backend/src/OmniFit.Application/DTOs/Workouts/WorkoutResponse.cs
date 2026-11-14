using System.Text.Json.Serialization;
using OmniFit.Domain.Enums;

namespace OmniFit.Application.DTOs.Workouts
{
    public record WorkoutResponse(
        Guid Id,
        string Name,
        [property: JsonConverter(typeof(JsonStringEnumConverter))]
        WorkoutStatus Status,
        DateTimeOffset? CompletedOn,
        IEnumerable<WorkoutExerciseResponse> WorkoutExercises
    );
}
