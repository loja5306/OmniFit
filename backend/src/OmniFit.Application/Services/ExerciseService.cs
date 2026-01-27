using OmniFit.Application.DTOs;
using OmniFit.Application.Interfaces;
using OmniFit.Application.Mapping;
using OmniFit.Domain.Entities;
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

        public async Task<IEnumerable<ExerciseResponse>> GetAllAsync()
        {
            var exercises = await _exerciseRepository.GetAllAsync();

            return exercises.Select(e => e.MapToResponse());
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
