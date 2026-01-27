using OmniFit.Application.DTOs;
using OmniFit.Domain.Entities;

namespace OmniFit.Application.Mapping
{
    public static class ExerciseMappingExtensions
    {
        public static Exercise MapToEntity(this CreateExerciseRequest request)
        {
            return new Exercise
            {
                Name = request.Name,
                Description = request.Description
            };
        }

        public static ExerciseResponse MapToResponse(this Exercise exercise)
        {
            return new ExerciseResponse(
                exercise.Id,
                exercise.Name,
                exercise.Description);
        }
    }
}
