namespace ConsoleApp1
{
	using System.Globalization;
	using Fluxera.Extensions.Hosting;
	using Fluxera.Extensions.Hosting.Modules;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;

	[PublicAPI]
	[UsedImplicitly]
	internal sealed class ConsoleApp1Module : ConfigureServicesModule
	{
		/// <inheritdoc />
		public override void ConfigureServices(IServiceConfigurationContext context)
		{
			CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("de-DE");
			CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("de-DE");

			context.Services.AddHostedService<ConsoleHostedService>();
			context.Services.AddSingleton<IWeatherService, WeatherService>();
			context.Services.AddOptions<WeatherSettings>().Bind(context.Configuration.GetSection("Weather"));
		}
	}
}
