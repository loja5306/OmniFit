namespace OmniFit.Domain.Entities
{
    public class WorkoutSet : BaseEntity
    {
        public int SetNumber { get; set; }
        public int Reps { get; set; }
        public double Weight { get; set; }

        public Guid WorkoutExerciseId { get; set; }
        public WorkoutExercise WorkoutExercise { get; set; } = null!;
    }
}
