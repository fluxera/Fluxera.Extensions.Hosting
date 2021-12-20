namespace Fluxera.Extensions.Hosting
{
	using System;
	using Fluxera.Extensions.DependencyInjection;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;

	[PublicAPI]
	public static class ServiceCollectionExtensions
	{
		public static IHostEnvironment GetHostEnvironment(this IServiceCollection services)
		{
			return services.GetObject<IHostEnvironment>();
		}

		public static IConfiguration GetConfiguration(this IServiceCollection services)
		{
			return services.GetObject<IConfiguration>();
		}

		public static void UpdateConfiguration(this IServiceCollection services)
		{
			IConfiguration configuration = services.BuildConfiguration();
			services.ReplaceSingleton(configuration);
		}

		private static IConfiguration BuildConfiguration(this IServiceCollection services)
		{
			IConfiguration baseConfiguration = services.GetObject<IConfiguration>();
			IConfigurationBuilder builder = new ConfigurationBuilder();

			builder.AddConfiguration(baseConfiguration);

			IConfiguration configuration = builder.Build();
			return configuration;
		}

		public static IServiceCollection AddApplicationLoader<TStartupModule>(this IServiceCollection services,
			IConfiguration configuration,
			IHostEnvironment environment,
			ILogger bootstrapperLogger,
			Action<IPluginConfigurationContext>? configurePlugins = null,
			ApplicationLoaderBuilderFunc? applicationLoaderFactory = null)
			where TStartupModule : class, IModule
		{
			// Add configuration.
			services.AddObjectAccessor(configuration, ObjectAccessorLifetime.Application);

			// Add the environment to the services.
			services.AddObjectAccessor(environment, ObjectAccessorLifetime.ConfigureServices);

			// Add the bootstrapper logger to the services.
			services.AddObjectAccessor(bootstrapperLogger, ObjectAccessorLifetime.ConfigureServices);

			// Build the application.
			IModularApplicationBuilder applicationBuilder = new ModularApplicationBuilder(typeof(TStartupModule), services);
			configurePlugins?.Invoke(new PluginConfigurationContext(services, applicationBuilder.PluginSources));
			applicationBuilder.Build(applicationLoaderFactory);

			return services;
		}
	}
}
