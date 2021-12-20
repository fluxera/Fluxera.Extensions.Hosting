namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;
	using Microsoft.AspNetCore.Mvc.Infrastructure;
	using Microsoft.Extensions.DependencyInjection;

	[PublicAPI]
	public sealed class WebModule : ConfigureServicesModule
	{
		public override void ConfigureServices(IServiceConfigurationContext context)
		{
			// Add the http context accessor service.
			context.Log(services => services.AddHttpContextAccessor());

			// Add the action context accessor.
			context.Log("AddActionContextAccessor",
				services => services.AddSingleton<IActionContextAccessor, ActionContextAccessor>());
		}
	}
}
