using Microsoft.EntityFrameworkCore;
using OmniFit.Domain.Common;
using OmniFit.Domain.Entities;
using OmniFit.Domain.Interfaces;
using OmniFit.Infrastructure.Data;
using OmniFit.Infrastructure.Extensions;

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

        public async Task<Workout?> GetByIdAsync(Guid id, string? userId = null)
        {
            var query = _context.Workouts
                .Include(w => w.WorkoutExercises)
                    .ThenInclude(we => we.Exercise)
                .Include(w => w.WorkoutExercises)
                    .ThenInclude(we => we.WorkoutSets)
                .Where(w => w.Id == id);

            if (userId != null)
            {
                query = query.Where(w => w.UserId == userId);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<PagedResult<Workout>> GetAllAsync(int page, int pageSize)
        {
            var query = _context.Workouts
                .Include(w => w.WorkoutExercises)
                    .ThenInclude(we => we.Exercise)
                .Include(w => w.WorkoutExercises)
                    .ThenInclude(we => we.WorkoutSets)
                .OrderByDescending(w => w.CreatedOn);

            var count = await query.CountAsync();

            var items = await query
                .ApplyPagination(page, pageSize)
                .ToListAsync();

            return new(items, page, pageSize, count);
        }
        
        public async Task<PagedResult<Workout>> GetByUserIdAsync(int page, int pageSize, string userId)
        {
            var query = _context.Workouts
                .Include(w => w.WorkoutExercises)
                    .ThenInclude(we => we.Exercise)
                .Include(w => w.WorkoutExercises)
                    .ThenInclude(we => we.WorkoutSets)
                .Where(w => w.UserId == userId)
                .OrderByDescending(w => w.CreatedOn);

            var count = await query.CountAsync();

            var items = await query
                .ApplyPagination(page, pageSize)
                .ToListAsync();

            return new(items, page, pageSize, count);
        }

        public async Task AddExerciseAsync(WorkoutExercise workoutExercise)
        {
            await _context.AddAsync(workoutExercise);
        }

        public void RemoveExercise(WorkoutExercise workoutExercise)
        {
            _context.Remove(workoutExercise);
        }

        public async Task AddSetAsync(WorkoutSet workoutSet)
        {
            await _context.AddAsync(workoutSet);
        }

        public void UpdateSet(WorkoutSet workoutSet)
        {
            _context.WorkoutSets.Update(workoutSet);
        }

        public void RemoveSet(WorkoutSet workoutSet)
        {
            _context.Remove(workoutSet);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
