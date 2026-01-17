using OmniFit.Application.DTOs;
using OmniFit.Application.Interfaces;
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
            var exercise = new Exercise
            {
                Name = request.Name,
                Description = request.Description
            };

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

            return exercises.Select(e => new ExerciseResponse(
                e.Id, 
                e.Name, 
                e.Description));
        }

        public async Task<ExerciseResponse?> GetByIdAsync(Guid id)
        {
            var exercise = await _exerciseRepository.GetByIdAsync(id);

            if (exercise == null) return null;

            return new ExerciseResponse(
                exercise.Id,
                exercise.Name,
                exercise.Description);
        }

        public async Task<ExerciseResponse?> UpdateAsync(Guid id, UpdateExerciseRequest request)
        {
            var exercise = await _exerciseRepository.GetByIdAsync(id);

            if (exercise == null) return null;

            exercise.Name = request.Name;
            exercise.Description = request.Description;

            _exerciseRepository.Update(exercise);

            await _exerciseRepository.SaveChangesAsync();

            return new ExerciseResponse(
                exercise.Id,
                exercise.Name,
                exercise.Description);
        }
    }
}
