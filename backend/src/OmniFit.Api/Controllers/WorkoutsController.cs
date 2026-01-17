using Microsoft.AspNetCore.Mvc;
using OmniFit.Application.DTOs;
using OmniFit.Application.Interfaces;

namespace OmniFit.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkoutsController : ControllerBase
    {
        private readonly IWorkoutService _workoutService;

        public WorkoutsController(IWorkoutService workoutService)
        {
            _workoutService = workoutService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var workouts = await _workoutService.GetAllWorkoutsAsync();

            return Ok(workouts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var workout = await _workoutService.GetWorkoutByIdAsync(id);

            return Ok(workout);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWorkoutRequest request)
        {
            var id = await _workoutService.CreateWorkoutAsync(request);

            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
    }
}
