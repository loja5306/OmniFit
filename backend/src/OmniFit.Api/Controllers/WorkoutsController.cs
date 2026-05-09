using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmniFit.Application.DTOs.Workouts;
using OmniFit.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace OmniFit.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkoutsController : ControllerBase
    {
        private readonly IWorkoutService _workoutService;
        private readonly IValidator<CreateWorkoutRequest> _createRequestValidator;
        private readonly IValidator<WorkoutQueryParameters> _queryParametersValidator;

        public WorkoutsController(IWorkoutService workoutService, IValidator<CreateWorkoutRequest> createRequestValidator, IValidator<WorkoutQueryParameters> queryParametersValidator)
        {
            _workoutService = workoutService;
            _createRequestValidator = createRequestValidator;
            _queryParametersValidator = queryParametersValidator;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] WorkoutQueryParameters request)
        {
            await _queryParametersValidator.ValidateAndThrowAsync(request);

            var workouts = await _workoutService.GetAllWorkoutsAsync(request);

            return Ok(workouts);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetForUser([FromQuery] WorkoutQueryParameters request)
        {
            await _queryParametersValidator.ValidateAndThrowAsync(request);

            var userId = User.FindFirst(JwtRegisteredClaimNames.NameId)?.Value;

            if (userId == null) return Unauthorized();

            var workouts = await _workoutService.GetWorkoutsByUserIdAsync(request, userId);

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

            await _createRequestValidator.ValidateAndThrowAsync(request);

            var id = await _workoutService.CreateWorkoutAsync(request, userId);

            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
    }
}
