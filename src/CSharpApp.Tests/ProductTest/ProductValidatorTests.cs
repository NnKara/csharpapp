using CSharpApp.Application.Helpers;
using CSharpApp.Core.Dtos.Product;

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
            var errors = CreateProductRequestValidator.ValidateCreateProdReq(null);
            Assert.Contains("Request body is null.", errors);
        }


        [Fact]
        public void Validate_WhenValid_ReturnsNoErrors()
        {
            var errors = CreateProductRequestValidator.ValidateCreateProdReq(ValidRequest());
            Assert.Empty(errors);
        }


        [Fact]
        public void Validate_WhenTitleMissing_AddsError()
        {
            var request = ValidRequest();
            request.Title = " ";
            var errors = CreateProductRequestValidator.ValidateCreateProdReq(request);
            Assert.Contains("Title is required.", errors);
        }


        [Fact]
        public void Validate_WhenDescriptionMissing_AddsError()
        {
            var request = ValidRequest();
            request.Description = "";
            var errors = CreateProductRequestValidator.ValidateCreateProdReq(request);
            Assert.Contains("Description is required.", errors);
        }


        [Fact]
        public void Validate_WhenPriceNotPositive_AddsError()
        {
            var request = ValidRequest();
            request.Price = 0;
            var errors = CreateProductRequestValidator.ValidateCreateProdReq(request);
            Assert.Contains("Price must be greater than 0.", errors);
        }


        [Fact]
        public void Validate_WhenCategoryIdNotPositive_AddsError()
        {
            var request = ValidRequest();
            request.CategoryId = -1;
            var errors = CreateProductRequestValidator.ValidateCreateProdReq(request);
            Assert.Contains("CategoryId must be greater than 0.", errors);
        }


        [Fact]
        public void Validate_WhenNoImages_AddsError()
        {
            var request = ValidRequest();
            request.Images = [];
            var errors = CreateProductRequestValidator.ValidateCreateProdReq(request);
            Assert.Contains("At least one image is required.", errors);
        }


        [Fact]
        public void Validate_WhenImagesContainEmptyString_AddsError()
        {
            var request = ValidRequest();
            request.Images = ["imageURL", "   "];
            var errors = CreateProductRequestValidator.ValidateCreateProdReq(request);
            Assert.Contains("Images cannot contain empty values.", errors);
        }


        [Fact]
        public void Validate_WhenManyValidationsFail_CollectsAllErrors()
        {
            var req = ValidRequest();
            req.Title = "";
            req.Price = 0;
            req.Images = [];
            var errors = CreateProductRequestValidator.ValidateCreateProdReq(req);
            Assert.Contains("Title is required.", errors);
            Assert.Contains("Price must be greater than 0.", errors);
            Assert.Contains("At least one image is required.", errors);
        }
    }
}
