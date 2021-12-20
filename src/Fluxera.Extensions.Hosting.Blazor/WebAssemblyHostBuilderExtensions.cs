namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Reflection;
	using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.FileProviders;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Hosting.Internal;
	using Microsoft.Extensions.Logging;

	internal static class WebAssemblyHostBuilderExtensions
	{
		public static WebAssemblyHostBuilder ConfigureFoundationDefaults(this WebAssemblyHostBuilder hostBuilder)
		{
			// Configure default services.
			hostBuilder.Services.AddLogging();
			hostBuilder.Services.AddOptions();

			return hostBuilder;
		}

		public static WebAssemblyHostBuilder ConfigureBlazorDefaults(this WebAssemblyHostBuilder hostBuilder)
		{
			BlazorHostEnvironment environment = new BlazorHostEnvironment(hostBuilder.HostEnvironment, Assembly.GetEntryAssembly()?.GetName().Name ?? string.Empty);

			// Register the environment.
			hostBuilder.Services.AddSingleton<IHostEnvironment>(environment);

			// Use the web assembly lifetime.
			hostBuilder.UseWebAssemblyLifetime();

			return hostBuilder;
		}

		public static WebAssemblyHostBuilder ConfigureHostBuilder(this WebAssemblyHostBuilder hostBuilder, Action<WebAssemblyHostBuilder> configureBuilder)
		{
			configureBuilder.Invoke(hostBuilder);

			return hostBuilder;
		}

		internal static WebAssemblyHostBuilder ConfigureApplicationLoader<TStartupModule>(this WebAssemblyHostBuilder hostBuilder,
			ILogger logger,
			Action<IPluginConfigurationContext>? configurePlugins = null,
			ApplicationLoaderBuilderFunc? applicationLoaderFactory = null)
			where TStartupModule : class, IModule
		{
			BlazorHostEnvironment environment = new BlazorHostEnvironment(hostBuilder.HostEnvironment, Assembly.GetEntryAssembly()?.GetName().Name ?? string.Empty);
			hostBuilder.Services.AddApplicationLoader<TStartupModule>(hostBuilder.Configuration, environment, logger, configurePlugins, applicationLoaderFactory);

			return hostBuilder;
		}

		private static WebAssemblyHostBuilder UseWebAssemblyLifetime(this WebAssemblyHostBuilder hostBuilder)
		{
			hostBuilder.Services.AddSingleton<IHostLifetime, WebAssemblyLifetime>();
			hostBuilder.Services.AddSingleton<IHostApplicationLifetime, ApplicationLifetime>();

			return hostBuilder;
		}

		private sealed class BlazorHostEnvironment : IHostEnvironment, IWebAssemblyHostEnvironment
		{
			private readonly IWebAssemblyHostEnvironment webAssemblyHostEnvironment;

			public BlazorHostEnvironment(IWebAssemblyHostEnvironment webAssemblyHostEnvironment, string applicationName)
			{
				this.webAssemblyHostEnvironment = webAssemblyHostEnvironment;
				this.ApplicationName = applicationName;
			}

			/// <inheritdoc />
			public string EnvironmentName
			{
				get => this.Environment;
				set => throw new NotSupportedException();
			}

			/// <inheritdoc />
			public string ApplicationName { get; set; }

			/// <inheritdoc />
			public string ContentRootPath
			{
				get => throw new NotSupportedException("ContentRootPath not supported in Blazor application.");
				set => throw new NotSupportedException("ContentRootPath not supported in Blazor application.");
			}

			/// <inheritdoc />
			public IFileProvider ContentRootFileProvider
			{
				get => throw new NotSupportedException("ContentRootFileProvider not supported in Blazor application.");
				set => throw new NotSupportedException("ContentRootFileProvider not supported in Blazor application.");
			}

			/// <inheritdoc />
			public string Environment => this.webAssemblyHostEnvironment.Environment;

			/// <inheritdoc />
			public string BaseAddress => this.webAssemblyHostEnvironment.BaseAddress;
		}
	}
}
