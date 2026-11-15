using Microsoft.AspNetCore.Identity;
using OmniFit.Domain.Entities;

namespace OmniFit.Api.Tests.Integration
{
    public static class TestData
    {
        public static class Users
        {
            public static IdentityUser AdminUser = new IdentityUser
            {
                Id = "7d249d9e-73ef-41fc-90bb-25f48195ad1d",
                Email = "lukeatkinson@gmail.com",
                UserName = "lukeatkinson@gmail.com"
            };
            public static IdentityUser StandardUser = new IdentityUser
            {
                Id = "0c1eeaa7-497f-419a-85e9-c915beff9bf6",
                Email = "standarduser@gmail.com",
                UserName = "standarduser@gmail.com"
            };
        }

        public static class Exercises
        {
            public static Exercise BenchPressExercise = new Exercise
            {
                Name = "Bench Press",
                Description = "Chest Exercise"
            };

            public static Exercise SquatExercise = new Exercise
            {
                Name = "Squat",
                Description = "Quad Exercise"
            };
        }
    }
}
