namespace OmniFit.Application.DTOs.Exercises
{
    public record ExerciseQueryParameters(
        int Page = 1, 
        int PageSize = 20
    );
}