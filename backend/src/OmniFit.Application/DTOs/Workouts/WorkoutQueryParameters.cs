namespace OmniFit.Application.DTOs.Workouts
{
    public record WorkoutQueryParameters(
        int Page = 1, 
        int PageSize = 20
    );
}