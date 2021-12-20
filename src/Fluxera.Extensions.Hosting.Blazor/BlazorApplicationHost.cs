namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Diagnostics;
	using System.Net.Http;
	using System.Threading.Tasks;
	using Fluxera.Guards;
	using JetBrains.Annotations;
	using Microsoft.AspNetCore.Components;
	using Microsoft.AspNetCore.Components.Web;
	using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;

	[PublicAPI]
	public abstract class BlazorApplicationHost<TStartupModule, TRootComponent> : IApplicationHost
		where TStartupModule : class, IModule
		where TRootComponent : class, IComponent
	{
		private ApplicationHostEvents events = new ApplicationHostEvents();
		private ILogger logger;

		protected string[] CommandLineArgs { get; private set; }

		/// <inheritdoc />
		public async Task RunAsync(string[] args)
		{
			Guard.Against.Null(args, nameof(args));

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

				this.logger.LogDebug("Host configuration starting.");

				// Create the host builder and configure it.
				WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
				builder.RootComponents.Add<TRootComponent>("#app");
				builder.RootComponents.Add<HeadOutlet>("head::after");
				builder.Services.AddScoped(_ => new HttpClient
				{
					BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
				});
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
				this.logger.LogDebug("Running host using '{HostLifetime}'.", hostLifetime.GetType().Name);

				// Stop the stopwatch and log the startup time.
				stopwatch.Stop();
				this.logger.LogDebug("Host configured in {Duration} ms.", stopwatch.ElapsedMilliseconds);

				this.events.OnHostCreated();

				// Run the host.
				await host.RunAsync();
			}
			catch(Exception ex)
			{
				this.events.OnHostCreationFailed(ex);
				this.logger.LogCritical(ex, "Application terminated unexpectedly.");
			}
			finally
			{
				this.events.Dispose();
			}
		}

		protected virtual void ConfigureHostBuilder(WebAssemblyHostBuilder builder)
		{
		}

		protected virtual void ConfigureApplicationHostEvents(ApplicationHostEvents applicationHostEvents)
		{
		}

		protected virtual void ConfigureApplicationPlugins(IPluginConfigurationContext context)
		{
		}

		private ILogger CreateLogger()
		{
			ILoggerFactory loggerFactory = this.CreateBootstrapperLoggerFactory();
			return loggerFactory.CreateLogger(ApplicationHost.LoggerName);
		}

		protected virtual ILoggerFactory CreateBootstrapperLoggerFactory()
		{
			ILoggerFactory loggerFactory = new ConsoleOutLoggerFactory();
			return loggerFactory;
		}
	}
}
