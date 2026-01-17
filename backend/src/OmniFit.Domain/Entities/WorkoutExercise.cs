namespace OmniFit.Domain.Entities
{
    public class WorkoutExercise : BaseEntity
    {
        public Guid WorkoutId { get; set; }
        public Workout Workout { get; set; } = null!;

        public Guid ExerciseId { get; set; }
        public Exercise Exercise { get; set; } = null!;

        public List<WorkoutSet> WorkoutSets { get; set; } = new();
    }
}
