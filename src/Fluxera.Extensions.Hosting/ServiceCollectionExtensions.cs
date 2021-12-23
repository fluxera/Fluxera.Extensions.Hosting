namespace Fluxera.Extensions.Hosting
{
	using System;
	using Fluxera.Extensions.DependencyInjection;
	using Fluxera.Extensions.Hosting.Modules;
	using Fluxera.Extensions.Hosting.Plugins;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;

	/// <summary>
	///     Extension methods to
	/// </summary>
	[PublicAPI]
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		///     Adds the application loader and necessary services to the service collection.
		/// </summary>
		/// <typeparam name="TStartupModule">The type of the startup module</typeparam>
		/// <param name="services">The service collection.</param>
		/// <param name="configuration">Teh application configuration.</param>
		/// <param name="environment">The application environment.</param>
		/// <param name="bootstrapperLogger">The bootstrapping logger.</param>
		/// <param name="configurePlugins">An optional action that configures the plugins.</param>
		/// <param name="applicationLoaderFactory">The optional application loader builder factory function.</param>
		/// <returns></returns>
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

		internal static void UpdateConfiguration(this IServiceCollection services)
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
	}
}
