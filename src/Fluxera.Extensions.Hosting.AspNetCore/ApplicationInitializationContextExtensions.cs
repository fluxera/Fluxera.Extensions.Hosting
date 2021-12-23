namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;
	using Microsoft.AspNetCore.Builder;

	/// <summary>
	///     Extension methods on the <see cref="IApplicationInitializationContext" /> type.
	/// </summary>
	[PublicAPI]
	public static class ApplicationInitializationContextExtensions
	{
		/// <summary>
		///     Gets the <see cref="IApplicationBuilder" /> from the <see cref="WebApplicationInitializationContext" />.
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public static WebApplication GetApplicationBuilder(this IApplicationInitializationContext context)
		{
			WebApplicationInitializationContext webContext = (WebApplicationInitializationContext)context;
			return (WebApplication)webContext.ApplicationBuilder;
		}
	}
}
