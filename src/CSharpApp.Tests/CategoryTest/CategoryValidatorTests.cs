using CSharpApp.Application.Helpers;
using CSharpApp.Core.Dtos.Category;

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
            var errors = CreateCategoryRequestValidator.ValidateCreateCategoryReq(null);
            Assert.Contains("Request body is null.", errors);
        }


        [Fact]
        public void Validate_WhenValid_ReturnsNoErrors()
        {
            var errors = CreateCategoryRequestValidator.ValidateCreateCategoryReq(ValidRequest());
            Assert.Empty(errors);
        }


        [Fact]
        public void Validate_WhenNameMissing_AddsError()
        {
            var request = ValidRequest();
            request.Name = " ";
            var errors = CreateCategoryRequestValidator.ValidateCreateCategoryReq(request);
            Assert.Contains("Name is required.", errors);
        }


        [Fact]
        public void Validate_WhenImageMissing_AddsError()
        {
            var request = ValidRequest();
            request.Image = " ";
            var errors = CreateCategoryRequestValidator.ValidateCreateCategoryReq(request);
            Assert.Contains("Image is required.", errors);
        }


        [Fact]
        public void Validate_WhenNameAndImageMissing_CollectsAllErrors()
        {
            var request = ValidRequest();
            request.Name = " ";
            request.Image = " ";
            var errors = CreateCategoryRequestValidator.ValidateCreateCategoryReq(request);
            Assert.Contains("Name is required.", errors);
            Assert.Contains("Image is required.", errors);
        }
    }
}
