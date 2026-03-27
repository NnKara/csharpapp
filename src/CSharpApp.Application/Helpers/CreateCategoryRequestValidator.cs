using CSharpApp.Core.Dtos.Category;

namespace CSharpApp.Application.Helpers
{
    public static class CreateCategoryRequestValidator
    {
        public static List<string> ValidateCreateCategoryReq(CreateCategoryRequest? request)
        {
            var errors = new List<string>();

            if (request is null) return ["Request body is null."];

            if (string.IsNullOrWhiteSpace(request.Name)) errors.Add("Name is required.");

            if (string.IsNullOrWhiteSpace(request.Image)) errors.Add("Image is required.");

            return errors;
        }
    }
}
