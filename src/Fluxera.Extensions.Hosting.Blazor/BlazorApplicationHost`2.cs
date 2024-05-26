namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Diagnostics;
	using System.Threading.Tasks;
	using Fluxera.Extensions.Hosting.Modules;
	using Fluxera.Extensions.Hosting.Plugins;
	using JetBrains.Annotations;
	using Microsoft.AspNetCore.Components;
	using Microsoft.AspNetCore.Components.Web;
	using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;

	/// <summary>
	///     An abstract base class for blazor applications.
	/// </summary>
	/// <typeparam name="TStartupModule">The startup module type.</typeparam>
	/// <typeparam name="TRootComponent">The root component type.</typeparam>
	[PublicAPI]
	public abstract class BlazorApplicationHost<TStartupModule, TRootComponent> : IApplicationHost
		where TStartupModule : class, IModule
		where TRootComponent : class, IComponent
	{
		private ApplicationHostEvents events = new ApplicationHostEvents();
		private ILogger logger;

		/// <summary>
		///     Gets the command line arguments.
		/// </summary>
		protected string[] CommandLineArgs { get; private set; }

		/// <inheritdoc />
		public async Task RunAsync(string[] args)
		{
			Guard.ThrowIfNull(args);

			try
			{
				// Start a stopwatch for measuring the startup time.
				Stopwatch stopwatch = Stopwatch.StartNew();

				this.CommandLineArgs = args;

				// Configure the host events instance.
				this.ConfigureApplicationHostEvents(this.events);

				this.events.OnHostCreating();

				// Create a logger as soon as possible to support early logging.
				this.logger = this.CreateLogger();

				this.logger.LogHostConfigurationStarting();

				// Create the host builder and configure it.
				WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
				builder.RootComponents.Add<TRootComponent>("#app");
				builder.RootComponents.Add<HeadOutlet>("head::after");
				builder
					.ConfigureFoundationDefaults()
					.ConfigureBlazorDefaults()
					.ConfigureHostBuilder(this.ConfigureHostBuilder)
					.ConfigureApplicationLoader<TStartupModule>(
						this.logger,
						this.ConfigureApplicationPlugins);

				// Build the host and run it.
				WebAssemblyHost host = builder.Build();

				// Initialize the application loader.
				IApplicationLoader applicationLoader = host.Services.GetRequiredService<IApplicationLoader>();
				applicationLoader.Initialize(new ApplicationLoaderInitializationContext(host.Services));

				IHostLifetime hostLifetime = host.Services.GetRequiredService<IHostLifetime>();
				this.logger.LogHostLifetime(hostLifetime.GetType().Name);

				// Stop the stopwatch and log the startup time.
				stopwatch.Stop();
				this.logger.LogHostConfigurationDuration(stopwatch.ElapsedMilliseconds);

				this.events.OnHostCreated();

				// Run the host.
				await host.RunAsync();
			}
			catch(Exception ex)
			{
				this.events.OnHostCreationFailed(ex);
				this.logger?.LogHostTerminatedUnexpectedly(ex);
			}
			finally
			{
				this.events.Dispose();
			}
		}

		/// <summary>
		///     Creates a <see cref="ILoggerFactory" />.
		/// </summary>
		/// <returns>The logger factory.</returns>
		protected virtual ILoggerFactory CreateBootstrapperLoggerFactory()
		{
			ILoggerFactory loggerFactory = new ConsoleOutLoggerFactory();
			return loggerFactory;
		}

		/// <summary>
		///     Configures the <see cref="IHostBuilder" /> instance.
		/// </summary>
		/// <param name="builder"></param>
		protected virtual void ConfigureHostBuilder(WebAssemblyHostBuilder builder)
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
			ILoggerFactory loggerFactory = this.CreateBootstrapperLoggerFactory();
			return loggerFactory.CreateLogger(ApplicationHost.LoggerName);
		}
	}
}
