

namespace CSharpApp.Core.Dtos.JwtAuth
{
    public sealed class LoginResponse
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }
    }
}
