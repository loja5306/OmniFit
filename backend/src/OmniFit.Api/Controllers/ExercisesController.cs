using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmniFit.Application.DTOs;
using OmniFit.Application.Interfaces;

namespace OmniFit.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExercisesController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;
        private readonly IValidator<CreateExerciseRequest> _createRequestvalidator;
        private readonly IValidator<UpdateExerciseRequest> _updateRequestvalidator;

        public ExercisesController(IExerciseService exerciseService, IValidator<CreateExerciseRequest> createRequestvalidator, IValidator<UpdateExerciseRequest> updateRequestvalidator)
        {
            _exerciseService = exerciseService;
            _createRequestvalidator = createRequestvalidator;
            _updateRequestvalidator = updateRequestvalidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var exercises = await _exerciseService.GetAllAsync();

            return Ok(exercises);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var exercise = await _exerciseService.GetByIdAsync(id);

            if (exercise == null)
            {
                return NotFound();
            }

            return Ok(exercise);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExerciseRequest request)
        {
            await _createRequestvalidator.ValidateAndThrowAsync(request);

            var id = await _exerciseService.CreateAsync(request);

            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateExerciseRequest request)
        {
            await _updateRequestvalidator.ValidateAndThrowAsync(request);

            var updatedExercise = await _exerciseService.UpdateAsync(id, request);

            if (updatedExercise == null)
            {
                return NotFound();
            }

            return Ok(updatedExercise);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleted = await _exerciseService.DeleteByIdAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
