using CSharpApp.Core.Dtos.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Application.Helpers
{
    public static class CreateProductRequestValidator
    {
        public static List<string> ValidateCreateProdReq(CreateProductRequest? request)
        {
            var errors = new List<string>();

            if (request is null) return ["Request body is null."];
            if (string.IsNullOrWhiteSpace(request.Title)) errors.Add("Title is required.");
            if (string.IsNullOrWhiteSpace(request.Description)) errors.Add("Description is required.");
            if (request.Price <= 0) errors.Add("Price must be greater than 0.");
            if (request.CategoryId <= 0) errors.Add("CategoryId must be greater than 0.");

            if (request.Images is null || request.Images.Count == 0)
                errors.Add("At least one image is required.");
            else if (request.Images.Any(x => string.IsNullOrWhiteSpace(x)))
                errors.Add("Images cannot contain empty values.");

            return errors;
        }
    }
}
