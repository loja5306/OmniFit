namespace OmniFit.Domain.Entities
{
    public class Workout : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public List<WorkoutExercise> WorkoutExercises { get; set; } = new();
    }
}
