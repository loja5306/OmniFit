using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmniFit.Application.DTOs.Exercises;
using OmniFit.Application.Interfaces;

namespace OmniFit.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExercisesController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;
        private readonly IValidator<CreateExerciseRequest> _createRequestValidator;
        private readonly IValidator<UpdateExerciseRequest> _updateRequestValidator;
        private readonly IValidator<ExerciseQueryParameters> _queryParametersValidator;

        public ExercisesController(IExerciseService exerciseService, IValidator<CreateExerciseRequest> createRequestValidator, IValidator<UpdateExerciseRequest> updateRequestValidator, IValidator<ExerciseQueryParameters> queryParametersValidator)
        {
            _exerciseService = exerciseService;
            _createRequestValidator = createRequestValidator;
            _updateRequestValidator = updateRequestValidator;
            _queryParametersValidator = queryParametersValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ExerciseQueryParameters request)
        {
            await _queryParametersValidator.ValidateAndThrowAsync(request);

            var exercises = await _exerciseService.GetAllAsync(request);

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
            await _createRequestValidator.ValidateAndThrowAsync(request);

            var id = await _exerciseService.CreateAsync(request);

            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateExerciseRequest request)
        {
            await _updateRequestValidator.ValidateAndThrowAsync(request);

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
