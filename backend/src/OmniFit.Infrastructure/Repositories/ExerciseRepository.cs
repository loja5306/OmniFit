using Microsoft.EntityFrameworkCore;
using OmniFit.Domain.Entities;
using OmniFit.Domain.Interfaces;
using OmniFit.Infrastructure.Data;

namespace OmniFit.Infrastructure.Repositories
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly AppDbContext _context;

        public ExerciseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Exercise exercise)
        {
            await _context.AddAsync(exercise);
        }


        public async Task<IEnumerable<Exercise>> GetAllAsync()
        {
            return await _context.Exercises
                .ToListAsync();
        }

        public async Task<Exercise?> GetByIdAsync(Guid id)
        {
            return await _context.Exercises
                .FirstOrDefaultAsync(e => e.Id == id);
        }
        
        public void Update(Exercise exercise)
        {
            _context.Exercises.Update(exercise);
        }

        public void Delete(Exercise exercise)
        {
            _context.Remove(exercise);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
