namespace Fluxera.Extensions.Hosting
{
	using System;
	using Fluxera.Extensions.Hosting.Modules;
	using Fluxera.Extensions.Hosting.Plugins;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.FileProviders;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;
	using Microsoft.Maui.ApplicationModel;
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
			MauiHostEnvironment environment = new MauiHostEnvironment();

			// Register the environment.
			hostBuilder.Services.AddSingleton<IHostEnvironment>(environment);

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
			MauiHostEnvironment environment = new MauiHostEnvironment();

			hostBuilder.Services.AddApplicationLoader<TStartupModule>(
				hostBuilder.Configuration, environment, logger, configurePlugins, applicationLoaderFactory);

			return hostBuilder;
		}

		private sealed class MauiHostEnvironment : IHostEnvironment
		{
			/// <inheritdoc />
			public string EnvironmentName
			{
				get => string.Empty;
				set => throw new NotSupportedException();
			}

			/// <inheritdoc />
			public string ApplicationName { get; set; } = AppInfo.Current.Name;

			/// <inheritdoc />
			public string ContentRootPath
			{
				get => throw new NotSupportedException("ContentRootPath not supported in MAUI application.");
				set => throw new NotSupportedException("ContentRootPath not supported in MAUI application.");
			}

			/// <inheritdoc />
			public IFileProvider ContentRootFileProvider
			{
				get => throw new NotSupportedException("ContentRootFileProvider not supported in MAUI application.");
				set => throw new NotSupportedException("ContentRootFileProvider not supported in MAUI application.");
			}
		}
	}
}
