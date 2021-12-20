namespace Fluxera.Extensions.Hosting
{
	using Fluxera.Guards;
	using JetBrains.Annotations;
	using Microsoft.AspNetCore.Http;

	[PublicAPI]
	public static class HttpRequestExtensions
	{
		public static bool CanAccept(this HttpRequest request, string contentType)
		{
			Guard.Against.Null(request, nameof(request));
			Guard.Against.Null(contentType, nameof(contentType));


			return request.Headers["Accept"].ToString().Contains(contentType);
		}
	}
}
