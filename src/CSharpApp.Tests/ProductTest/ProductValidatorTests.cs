using CSharpApp.Application.Products.Commands;
using CSharpApp.Core.Dtos.Product;
using FluentValidation;

namespace CSharpApp.Tests.ProductTest
{
    public class ProductValidatorTests
    {

        private static CreateProductRequest ValidRequest()
        {
            return new CreateProductRequest
            {
                Title = "Title",
                Description = "Description",
                Price = 10,
                CategoryId = 1,
                Images = new List<string> { "imageURL" }
            };
        }


        [Fact]
        public void Validate_WhenRequestIsNull_AddsError()
        {
            var validator = new CreateProductRequestValidator();
            Assert.Throws<ArgumentNullException>(() => validator.Validate((CreateProductRequest)null!));
        }


        [Fact]
        public void Validate_WhenValid_ReturnsNoErrors()
        {
            var validator = new CreateProductRequestValidator();
            var result = validator.Validate(ValidRequest());
            Assert.True(result.IsValid);
        }


        [Fact]
        public void Validate_WhenTitleMissing_AddsError()
        {
            var request = ValidRequest();
            request.Title = " ";
            var validator = new CreateProductRequestValidator();
            var result = validator.Validate(request);
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Title is required.");
        }


        [Fact]
        public void Validate_WhenDescriptionMissing_AddsError()
        {
            var request = ValidRequest();
            request.Description = "";
            var validator = new CreateProductRequestValidator();
            var result = validator.Validate(request);
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Description is required.");
        }


        [Fact]
        public void Validate_WhenPriceNotPositive_AddsError()
        {
            var request = ValidRequest();
            request.Price = 0;
            var validator = new CreateProductRequestValidator();
            var result = validator.Validate(request);
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Price must be greater than 0.");
        }


        [Fact]
        public void Validate_WhenCategoryIdNotPositive_AddsError()
        {
            var request = ValidRequest();
            request.CategoryId = -1;
            var validator = new CreateProductRequestValidator();
            var result = validator.Validate(request);
            Assert.Contains(result.Errors, e => e.ErrorMessage == "CategoryId must be greater than 0.");
        }


        [Fact]
        public void Validate_WhenNoImages_AddsError()
        {
            var request = ValidRequest();
            request.Images = [];
            var validator = new CreateProductRequestValidator();
            var result = validator.Validate(request);
            Assert.Contains(result.Errors, e => e.ErrorMessage == "At least one image is required.");
        }


        [Fact]
        public void Validate_WhenImagesContainEmptyString_AddsError()
        {
            var request = ValidRequest();
            request.Images = ["imageURL", "   "];
            var validator = new CreateProductRequestValidator();
            var result = validator.Validate(request);
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Images cannot contain empty values.");
        }


        [Fact]
        public void Validate_WhenManyValidationsFail_CollectsAllErrors()
        {
            var req = ValidRequest();
            req.Title = "";
            req.Price = 0;
            req.Images = [];
            var validator = new CreateProductRequestValidator();
            var result = validator.Validate(req);
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Title is required.");
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Price must be greater than 0.");
            Assert.Contains(result.Errors, e => e.ErrorMessage == "At least one image is required.");
        }
    }
}
