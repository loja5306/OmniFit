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
        private readonly IValidator<WorkoutRequest> _workoutRequestValidator;
        private readonly IValidator<WorkoutQueryParameters> _queryParametersValidator;
        private readonly IValidator<WorkoutExerciseRequest> _workoutExerciseRequestValidator;
        private readonly IValidator<WorkoutSetRequest> _workoutSetRequestValidator;

        public WorkoutsController(IWorkoutService workoutService, IValidator<WorkoutRequest> workoutRequestValidator, IValidator<WorkoutQueryParameters> queryParametersValidator, IValidator<WorkoutExerciseRequest> workoutExerciseRequestValidator, IValidator<WorkoutSetRequest> workoutSetRequestValidator)
        {
            _workoutService = workoutService;
            _workoutRequestValidator = workoutRequestValidator;
            _queryParametersValidator = queryParametersValidator;
            _workoutExerciseRequestValidator = workoutExerciseRequestValidator;
            _workoutSetRequestValidator = workoutSetRequestValidator;
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
            var userId = User.FindFirst(JwtRegisteredClaimNames.NameId)?.Value;
            if (userId == null) return Unauthorized();

            var workout = await _workoutService.GetWorkoutByIdAsync(id, User.IsInRole("Admin") ? null : userId);

            if (workout == null)
            {
                return NotFound();
            }

            return Ok(workout);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WorkoutRequest request)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.NameId)?.Value;
            if (userId == null) return Unauthorized();

            await _workoutRequestValidator.ValidateAndThrowAsync(request);

            var id = await _workoutService.CreateWorkoutAsync(request, userId);

            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [Authorize]
        [HttpPost("{workoutId}/Exercises")]
        public async Task<IActionResult> AddExercise([FromRoute] Guid workoutId, [FromBody] WorkoutExerciseRequest request)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.NameId)?.Value;
            if (userId == null) return Unauthorized();

            await _workoutExerciseRequestValidator.ValidateAndThrowAsync(request);

            var id = await _workoutService.AddExerciseAsync(workoutId, request, userId);
            if (id == null) return NotFound();

            return CreatedAtAction(nameof(GetById), new { id = workoutId }, id);
        }

        [Authorize]
        [HttpDelete("{workoutId}/Exercises/{workoutExerciseId}")]
        public async Task<IActionResult> RemoveExercise([FromRoute] Guid workoutId, [FromRoute] Guid workoutExerciseId)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.NameId)?.Value;
            if (userId == null) return Unauthorized();

            var deleted = await _workoutService.RemoveExerciseAsync(workoutId, workoutExerciseId, userId);

            if (!deleted) return NotFound();

            return NoContent();
        }

        [Authorize]
        [HttpPost("{workoutId}/Exercises/{workoutExerciseId}/Sets")]
        public async Task<IActionResult> AddSet([FromRoute] Guid workoutId, [FromRoute] Guid workoutExerciseId, [FromBody] WorkoutSetRequest request)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.NameId)?.Value;
            if (userId == null) return Unauthorized();

            await _workoutSetRequestValidator.ValidateAndThrowAsync(request);

            var id = await _workoutService.AddSetAsync(workoutId, workoutExerciseId, request, userId);
            if (id == null) return NotFound();

            return CreatedAtAction(nameof(GetById), new { id = workoutId }, id);
        }

        [Authorize]
        [HttpPut("{workoutId}/Exercises/{workoutExerciseId}/Sets/{workoutSetId}")]
        public async Task<IActionResult> UpdateSet([FromRoute] Guid workoutId, [FromRoute] Guid workoutExerciseId, [FromRoute] Guid workoutSetId, [FromBody] WorkoutSetRequest request)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.NameId)?.Value;
            if (userId == null) return Unauthorized();

            await _workoutSetRequestValidator.ValidateAndThrowAsync(request);

            var updatedWorkoutSet = await _workoutService.UpdateSetAsync(workoutId, workoutExerciseId, workoutSetId, request, userId);

            if (updatedWorkoutSet == null)
            {
                return NotFound();
            }

            return Ok(updatedWorkoutSet);
        }

        [Authorize]
        [HttpDelete("{workoutId}/Exercises/{workoutExerciseId}/Sets/{workoutSetId}")]
        public async Task<IActionResult> RemoveSet([FromRoute] Guid workoutId, [FromRoute] Guid workoutExerciseId, [FromRoute] Guid workoutSetId)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.NameId)?.Value;
            if (userId == null) return Unauthorized();

            var deleted = await _workoutService.RemoveSetAsync(workoutId, workoutExerciseId, workoutSetId, userId);

            if (!deleted) return NotFound();

            return NoContent();
        }

        [Authorize]
        [HttpPost("{workoutId}/Complete")]
        public async Task<IActionResult> Complete([FromRoute] Guid workoutId)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.NameId)?.Value;
            if (userId == null) return Unauthorized();

            var completed = await _workoutService.CompleteWorkoutAsync(workoutId, userId);
            if (!completed) return NotFound();

            return NoContent();
        }
    }
}
