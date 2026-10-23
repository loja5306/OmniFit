using OmniFit.Domain.Enums;

namespace OmniFit.Domain.Entities
{
    public class Workout : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public WorkoutStatus Status { get; set; } = WorkoutStatus.InProgress;
        public DateTimeOffset? CompletedOn { get; set; }
        public List<WorkoutExercise> WorkoutExercises { get; set; } = new();
    }
}
