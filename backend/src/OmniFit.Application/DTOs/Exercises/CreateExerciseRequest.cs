namespace OmniFit.Application.DTOs.Exercises
{
    public record CreateExerciseRequest (
        string Name, 
        string Description
    );
}
