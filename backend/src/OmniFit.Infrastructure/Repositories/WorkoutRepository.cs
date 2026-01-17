using Microsoft.EntityFrameworkCore;
using OmniFit.Domain.Entities;
using OmniFit.Domain.Interfaces;
using OmniFit.Infrastructure.Data;

namespace OmniFit.Infrastructure.Repositories
{
    public class WorkoutRepository : IWorkoutRepository
    {
        private readonly AppDbContext _context;

        public WorkoutRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Workout workout)
        {
            await _context.AddAsync(workout);
        }

        public async Task<IEnumerable<Workout>> GetAllAsync()
        {
            return await _context.Workouts
                .Include(w => w.WorkoutExercises)
                    .ThenInclude(we => we.Exercise)
                .Include(w => w.WorkoutExercises)
                    .ThenInclude(we => we.WorkoutSets)
                .OrderByDescending(w => w.CreatedOn)
                .ToListAsync();
        }

        public async Task<Workout?> GetByIdAsync(Guid id)
        {
            return await _context.Workouts
                .Include(w => w.WorkoutExercises)
                    .ThenInclude(we => we.Exercise)
                .Include(w => w.WorkoutExercises)
                    .ThenInclude(we => we.WorkoutSets)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
