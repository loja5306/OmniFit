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

        public ExercisesController(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExerciseRequest request)
        {
            var id = await _exerciseService.CreateAsync(request);

            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateExerciseRequest request)
        {
            var updatedExercise = await _exerciseService.UpdateAsync(id, request);

            if (updatedExercise == null)
            {
                return NotFound();
            }

            return Ok(updatedExercise);
        }

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
