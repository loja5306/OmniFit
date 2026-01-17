namespace OmniFit.Domain.Entities
{
    public class Exercise : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public List<WorkoutExercise> WorkoutExercise { get; set; } = new();
    }
}
