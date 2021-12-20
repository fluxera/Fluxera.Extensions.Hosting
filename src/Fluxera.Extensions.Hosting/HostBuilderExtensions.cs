namespace Fluxera.Extensions.Hosting
{
	using System;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;

	/// <summary>
	///     Extension methods for the <see cref="IHostBuilder" /> type.
	/// </summary>
	[PublicAPI]
	public static class HostBuilderExtensions
	{
		public static IHostBuilder ConfigureFoundationDefaults(this IHostBuilder hostBuilder)
		{
			// Add special custom environment variables to the host configuration.
			hostBuilder.ConfigureHostConfiguration(builder =>
			{
				builder.AddEnvironmentVariables("FLUXERA_");
			});

			// Add special custom environment variables to the application configuration.
			hostBuilder.ConfigureAppConfiguration(builder =>
			{
				builder.AddEnvironmentVariables("FLUXERA_");
			});

			// Configure default services.
			hostBuilder.ConfigureServices(services =>
			{
				services.AddLogging();
				services.AddOptions();
			});

			return hostBuilder;
		}

		public static IHostBuilder ConfigureHostBuilder(this IHostBuilder hostBuilder, Action<IHostBuilder> configureBuilder)
		{
			configureBuilder.Invoke(hostBuilder);

			return hostBuilder;
		}

		public static IHostBuilder ConfigureApplicationLoader<TStartupModule>(this IHostBuilder hostBuilder,
			ILogger logger,
			Action<IPluginConfigurationContext>? configurePlugins = null,
			ApplicationLoaderBuilderFunc? applicationLoaderFactory = null)
			where TStartupModule : class, IModule
		{
			hostBuilder.ConfigureServices((context, services) =>
			{
				services.AddApplicationLoader<TStartupModule>(context.Configuration, context.HostingEnvironment, logger, configurePlugins, applicationLoaderFactory);
			});

			return hostBuilder;
		}
	}
}
