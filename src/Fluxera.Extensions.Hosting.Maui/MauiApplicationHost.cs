namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Diagnostics;
	using Fluxera.Extensions.Hosting.Modules;
	using Fluxera.Extensions.Hosting.Plugins;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;
	using Microsoft.Maui.Controls;
	using Microsoft.Maui.Controls.Hosting;
	using Microsoft.Maui.Hosting;

	/// <summary>
	///     A static entry-point the a MAUI application.
	/// </summary>
	[PublicAPI]
	public static class MauiApplicationHost
	{
		/// <summary>
		///     Creates the application.
		/// </summary>
		/// <typeparam name="TApplicationHost">The type of the host.</typeparam>
		/// <returns></returns>
		public static MauiApp BuildApplication<TApplicationHost>()
			where TApplicationHost : class, IMauiApplicationHost, new()
		{
			IMauiApplicationHost applicationHost = new TApplicationHost();
			return applicationHost.Build();
		}
	}

	/// <summary>
	///     An abstract base class for MAUI host applications.
	/// </summary>
	/// <typeparam name="TStartupModule"></typeparam>
	/// <typeparam name="TApplication"></typeparam>
	[PublicAPI]
	public abstract class MauiApplicationHost<TStartupModule, TApplication> : IMauiApplicationHost
		where TStartupModule : class, IModule
		where TApplication : Application
	{
		private ApplicationHostEvents events = new ApplicationHostEvents();
		private ILogger logger;

		/// <inheritdoc />
		MauiApp IMauiApplicationHost.Build()
		{
			MauiApp app = null;

			try
			{
				// Start a stopwatch for measuring the startup time.
				Stopwatch stopwatch = Stopwatch.StartNew();

				// Configure the host events instance.
				this.ConfigureApplicationHostEvents(this.events);

				this.events.OnHostCreating();

				// Create a logger as soon as possible to support early logging.
				this.logger = this.CreateLogger();

				this.logger.LogHostConfigurationStarting();

				// Create the host builder and configure it.
				MauiAppBuilder builder = MauiApp.CreateBuilder();
				builder
					.UseMauiApp<TApplication>()
					.ConfigureFoundationDefaults()
					.ConfigureMauiDefaults()
					.ConfigureHostBuilder(this.ConfigureHostBuilder)
					.ConfigureApplicationLoader<TStartupModule>(
						this.logger,
						this.ConfigureApplicationPlugins);

				// Build the app.
				app = builder.Build();

				// Initialize the application loader.
				IApplicationLoader applicationLoader = app.Services.GetRequiredService<IApplicationLoader>();
				applicationLoader.Initialize(new ApplicationLoaderInitializationContext(app.Services));

				IHostLifetime hostLifetime = app.Services.GetRequiredService<IHostLifetime>();
				this.logger.LogHostLifetime(hostLifetime.GetType().Name);

				// Stop the stopwatch and log the startup time.
				stopwatch.Stop();
				this.logger.LogHostConfigurationDuration(stopwatch.ElapsedMilliseconds);

				this.events.OnHostCreated();
			}
			catch(Exception ex)
			{
				this.events.OnHostCreationFailed(ex);
				this.logger?.LogHostTerminatedUnexpectedly(ex);
				System.Diagnostics.Trace.WriteLine(ex);
				System.Diagnostics.Debug.WriteLine(ex);
				Console.Error.WriteLine(ex);
			}
			finally
			{
				this.events.Dispose();
			}

			return app;
		}

		/// <summary>
		///     Creates a <see cref="ILoggerFactory" />.
		/// </summary>
		/// <returns>The logger factory.</returns>
		protected virtual ILoggerFactory CreateBootstrapperLoggerFactory( /*IConfiguration configuration*/)
		{
			ILoggerFactory loggerFactory = LoggerFactory.Create(loggingBuilder =>
			{
				//loggingBuilder.AddConfiguration(configuration.GetSection("Logging"));
				loggingBuilder.AddConsole();
			});

			return loggerFactory;
		}

		/// <summary>
		///     Configures the <see cref="IHostBuilder" /> instance.
		/// </summary>
		/// <param name="builder"></param>
		protected virtual void ConfigureHostBuilder(MauiAppBuilder builder)
		{
		}

		/// <summary>
		///     Configures optional event handlers on the given <see cref="ApplicationHostEvents" /> instance.
		/// </summary>
		/// <param name="applicationHostEvents"></param>
		protected virtual void ConfigureApplicationHostEvents(ApplicationHostEvents applicationHostEvents)
		{
		}

		/// <summary>
		///     Configures the plugin modules of the application.
		/// </summary>
		/// <param name="context"></param>
		protected virtual void ConfigureApplicationPlugins(IPluginConfigurationContext context)
		{
		}

		private ILogger CreateLogger()
		{
			//IConfiguration configuration = new ConfigurationBuilder()
			//	.SetFileProvider(new EmbeddedFileProvider(typeof(TApplication).Assembly))
			//	.AddInMemoryCollection() // Make sure there's some default storage since there are no default providers.
			//	.AddJsonFile("appsettings.json", true)
			//	.AddJsonFile($"appsettings.{this.environment.EnvironmentName}.json", true)
			//	.Build();

			ILoggerFactory loggerFactory = this.CreateBootstrapperLoggerFactory( /*configuration*/);
			return loggerFactory.CreateLogger(ApplicationHost.LoggerName);
		}
	}
}
