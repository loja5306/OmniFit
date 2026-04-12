using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmniFit.Application.DTOs;
using OmniFit.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace OmniFit.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkoutsController : ControllerBase
    {
        private readonly IWorkoutService _workoutService;
        private readonly IValidator<CreateWorkoutRequest> _createRequestvalidator;

        public WorkoutsController(IWorkoutService workoutService, IValidator<CreateWorkoutRequest> createRequestvalidator)
        {
            _workoutService = workoutService;
            _createRequestvalidator = createRequestvalidator;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var workouts = await _workoutService.GetAllWorkoutsAsync();

            return Ok(workouts);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetForUser()
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.NameId)?.Value;

            if (userId == null) return Unauthorized();

            var workouts = await _workoutService.GetWorkoutsByUserIdAsync(userId);

            return Ok(workouts);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var workout = await _workoutService.GetWorkoutByIdAsync(id);

            if (workout == null)
            {
                return NotFound();
            }

            return Ok(workout);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWorkoutRequest request)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.NameId)?.Value;

            if (userId == null) return Unauthorized();

            await _createRequestvalidator.ValidateAndThrowAsync(request);

            var id = await _workoutService.CreateWorkoutAsync(request, userId);

            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
    }
}
