using CSharpApp.Application.Categories.Commands;
using CSharpApp.Core.Dtos.Category;
using FluentValidation;

namespace CSharpApp.Tests.CategoryTest
{
    public class CategoryValidatorTests
    {

        private static CreateCategoryRequest ValidRequest()
        {
            return new CreateCategoryRequest
            {
                Name = "Name",
                Image = "imageURL"
            };
        }


        [Fact]
        public void Validate_WhenRequestIsNull_AddsError()
        {
            var validator = new CreateCategoryRequestValidator();
            CreateCategoryRequest? request = null;
            Assert.Throws<ArgumentNullException>(() => validator.Validate(request!));
        }


        [Fact]
        public void Validate_WhenValid_ReturnsNoErrors()
        {
            var validator = new CreateCategoryRequestValidator();
            var result = validator.Validate(ValidRequest());
            Assert.True(result.IsValid);
        }


        [Fact]
        public void Validate_WhenNameMissing_AddsError()
        {
            var request = ValidRequest();
            request.Name = " ";
            var validator = new CreateCategoryRequestValidator();
            var result = validator.Validate(request);
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Name is required.");
        }


        [Fact]
        public void Validate_WhenImageMissing_AddsError()
        {
            var request = ValidRequest();
            request.Image = " ";
            var validator = new CreateCategoryRequestValidator();
            var result = validator.Validate(request);
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Image is required.");
        }


        [Fact]
        public void Validate_WhenNameAndImageMissing_CollectsAllErrors()
        {
            var request = ValidRequest();
            request.Name = " ";
            request.Image = " ";
            var validator = new CreateCategoryRequestValidator();
            var result = validator.Validate(request);
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Name is required.");
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Image is required.");
        }
    }
}
