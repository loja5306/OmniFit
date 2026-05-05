using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OmniFit.Domain.Entities;

namespace OmniFit.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Workout> Workouts => Set<Workout>();
        public DbSet<Exercise> Exercises => Set<Exercise>();
        public DbSet<WorkoutExercise> WorkoutExercises => Set<WorkoutExercise>();
        public DbSet<WorkoutSet> WorkoutSets => Set<WorkoutSet>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Workout>(e =>
            {
                e.HasKey(w => w.Id);
                e.HasOne<IdentityUser>()
                    .WithMany()
                    .HasForeignKey(w => w.UserId);
            });

            modelBuilder.Entity<Exercise>(e =>
            {
                e.HasKey(w => w.Id);
            });

            modelBuilder.Entity<WorkoutExercise>(e =>
            {
                e.HasKey(we => we.Id);

                e.HasOne(we => we.Workout)
                    .WithMany(w => w.WorkoutExercises)
                    .HasForeignKey(we => we.WorkoutId);

                e.HasOne(we => we.Exercise)
                    .WithMany(e => e.WorkoutExercise)
                    .HasForeignKey(we => we.ExerciseId);
            });

            modelBuilder.Entity<WorkoutSet>(e =>
            {
                e.HasKey(we => we.Id);

                e.HasOne(ws => ws.WorkoutExercise)
                    .WithMany(we => we.WorkoutSets)
                    .HasForeignKey(ws => ws.WorkoutExerciseId);
            });

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "a4a244a0-8f9d-4c16-843b-fb121ef0bb88",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "b75d4aee-0984-4b23-982a-3618decec41b"
                }
            );
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity &&
                    (e.State == EntityState.Added) || (e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                ((BaseEntity)entry.Entity).UpdatedOn = DateTimeOffset.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    ((BaseEntity)entry.Entity).CreatedOn = DateTimeOffset.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
