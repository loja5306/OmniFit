
using FluentValidation.TestHelper;
using OmniFit.Application.DTOs.Exercises;
using OmniFit.Application.Validators.Exercises;

namespace OmniFit.Application.Tests.Unit.Validators
{
    public class ExerciseQueryParametersTests
    {
        private readonly ExerciseQueryParametersValidator _sut = new();

        [Fact]
        public void ExerciseQueryParameters_ShouldThrowError_WhenPageIsInvalid()
        {
            //Arrange
            var request = new ExerciseQueryParameters(0, 20);

            //Act
            var result = _sut.TestValidate(request);
            
            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Page)
                .WithErrorMessage("Page must be greater than or equal to 1");
        }

        [Fact]
        public void ExerciseQueryParameters_ShouldThrowError_WhenPageSizeIsInvalid()
        {
            //Arrange
            var request = new ExerciseQueryParameters(1, 200);

            //Act
            var result = _sut.TestValidate(request);
            
            //Assert
            result.ShouldHaveValidationErrorFor(x => x.PageSize)
                .WithErrorMessage("Page size must be between 1 and 100");
        }
    }
}