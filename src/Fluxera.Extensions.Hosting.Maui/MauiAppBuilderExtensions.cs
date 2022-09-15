namespace Fluxera.Extensions.Hosting
{
	using System;
	using Fluxera.Extensions.Hosting.Modules;
	using Fluxera.Extensions.Hosting.Plugins;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;
	using Microsoft.Maui.Hosting;

	internal static class MauiAppBuilderExtensions
	{
		public static MauiAppBuilder ConfigureFoundationDefaults(this MauiAppBuilder hostBuilder)
		{
			// Configure default services.
			hostBuilder.Services.AddLogging();
			hostBuilder.Services.AddOptions();

			return hostBuilder;
		}

		public static MauiAppBuilder ConfigureMauiDefaults(this MauiAppBuilder hostBuilder)
		{
			//MauiHostEnvironment environment = new MauiHostEnvironment(hostBuilder.HostEnvironment, Assembly.GetEntryAssembly()?.GetName().Name ?? string.Empty);

			//// Register the environment.
			//hostBuilder.Services.AddSingleton<IHostEnvironment>(environment);

			//// Use the web assembly lifetime.
			//hostBuilder.UseWebAssemblyLifetime();

			return hostBuilder;
		}

		public static MauiAppBuilder ConfigureHostBuilder(this MauiAppBuilder hostBuilder, Action<MauiAppBuilder> configureBuilder)
		{
			configureBuilder.Invoke(hostBuilder);

			return hostBuilder;
		}

		public static MauiAppBuilder ConfigureApplicationLoader<TStartupModule>(this MauiAppBuilder hostBuilder,
			ILogger logger,
			Action<IPluginConfigurationContext> configurePlugins = null,
			ApplicationLoaderBuilderFunc applicationLoaderFactory = null)
			where TStartupModule : class, IModule
		{
			hostBuilder.Services.AddApplicationLoader<TStartupModule>(
				hostBuilder.Configuration, null, logger, configurePlugins, applicationLoaderFactory);

			return hostBuilder;
		}
	}
}
