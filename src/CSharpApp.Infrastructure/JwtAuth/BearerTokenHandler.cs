using System.Net.Http.Headers;

namespace CSharpApp.Infrastructure.JwtAuth
{
    public sealed class BearerTokenHandler : DelegatingHandler
    {
        private readonly IAuthTokenProvider _tokenProvider;
        public BearerTokenHandler(IAuthTokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,CancellationToken cancellationToken)
        {
            var token = await _tokenProvider.GetAccessTokenAsync(cancellationToken);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await base.SendAsync(request, cancellationToken);
        }     
    }
}
