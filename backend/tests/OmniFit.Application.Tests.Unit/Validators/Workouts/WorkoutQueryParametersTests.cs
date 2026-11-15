
using FluentValidation.TestHelper;
using OmniFit.Application.DTOs.Workouts;
using OmniFit.Application.Validators.Workouts;

namespace OmniFit.Application.Tests.Unit.Validators
{
    public class WorkoutQueryParametersTests
    {
        private readonly WorkoutQueryParametersValidator _sut = new();

        [Fact]
        public void WorkoutQueryParameters_ShouldThrowError_WhenPageIsInvalid()
        {
            //Arrange
            var request = new WorkoutQueryParameters(0, 20);

            //Act
            var result = _sut.TestValidate(request);
            
            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Page)
                .WithErrorMessage("Page must be greater than or equal to 1");
        }

        [Fact]
        public void WorkoutQueryParameters_ShouldThrowError_WhenPageSizeIsInvalid()
        {
            //Arrange
            var request = new WorkoutQueryParameters(1, 200);

            //Act
            var result = _sut.TestValidate(request);
            
            //Assert
            result.ShouldHaveValidationErrorFor(x => x.PageSize)
                .WithErrorMessage("Page size must be between 1 and 100");
        }
    }
}