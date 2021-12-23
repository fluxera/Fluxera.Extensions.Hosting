namespace Fluxera.Extensions.Hosting
{
	using Fluxera.Extensions.Hosting.Modules;
	using JetBrains.Annotations;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc.Infrastructure;
	using Microsoft.Extensions.DependencyInjection;

	/// <summary>
	///     A module that configures the basic web application services <see cref="IHttpContextAccessor" />
	///     and the <see cref="IActionContextAccessor" />.
	/// </summary>
	[PublicAPI]
	public sealed class WebModule : ConfigureServicesModule
	{
		/// <inheritdoc />
		public override void ConfigureServices(IServiceConfigurationContext context)
		{
			// Add the http context accessor.
			context.Log(services => services.AddHttpContextAccessor());

			// Add the action context accessor.
			context.Log("AddActionContextAccessor",
				services => services.AddSingleton<IActionContextAccessor, ActionContextAccessor>());
		}
	}
}
