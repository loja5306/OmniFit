using OmniFit.Application.DTOs.Exercises;
using OmniFit.Application.Interfaces;
using OmniFit.Application.Mapping;
using OmniFit.Domain.Common;
using OmniFit.Domain.Interfaces;

namespace OmniFit.Application.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseRepository _exerciseRepository;

        public ExerciseService(IExerciseRepository exerciseRepository)
        {
            _exerciseRepository = exerciseRepository;
        }

        public async Task<Guid> CreateAsync(CreateExerciseRequest request)
        {
            var exercise = request.MapToEntity();

            await _exerciseRepository.AddAsync(exercise);
            await _exerciseRepository.SaveChangesAsync();

            return exercise.Id;
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            var exercise = await _exerciseRepository.GetByIdAsync(id);

            if (exercise == null) return false;

            _exerciseRepository.Delete(exercise);
            await _exerciseRepository.SaveChangesAsync();

            return true;
        }

        public async Task<PagedResult<ExerciseResponse>> GetAllAsync(ExerciseQueryParameters request)
        {
            var exercises = await _exerciseRepository.GetAllAsync(request.Page, request.PageSize);

            return exercises.MapToResponse();
        }

        public async Task<ExerciseResponse?> GetByIdAsync(Guid id)
        {
            var exercise = await _exerciseRepository.GetByIdAsync(id);

            if (exercise == null) return null;

            return exercise.MapToResponse();
        }

        public async Task<ExerciseResponse?> UpdateAsync(Guid id, UpdateExerciseRequest request)
        {
            var exercise = await _exerciseRepository.GetByIdAsync(id);

            if (exercise == null) return null;

            exercise.Name = request.Name;
            exercise.Description = request.Description;

            _exerciseRepository.Update(exercise);

            await _exerciseRepository.SaveChangesAsync();

            return exercise.MapToResponse();
        }
    }
}
