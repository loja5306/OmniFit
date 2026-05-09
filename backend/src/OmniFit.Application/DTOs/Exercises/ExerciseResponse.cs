namespace OmniFit.Application.DTOs.Exercises
{
    public record ExerciseResponse (
        Guid Id, 
        string Name, 
        string Description
    );
}
