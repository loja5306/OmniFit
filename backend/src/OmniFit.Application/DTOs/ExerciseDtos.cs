namespace OmniFit.Application.DTOs
{
    public record ExerciseResponse (Guid Id, string Name, string Description);

    public record CreateExerciseRequest (string Name, string Description);
    public record UpdateExerciseRequest (string Name, string Description);
}
