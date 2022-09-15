namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Diagnostics;
	using System.IO;
	using System.Reflection;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Extensions.Hosting.Modules;
	using Fluxera.Extensions.Hosting.Plugins;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.FileProviders;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Hosting.Internal;
	using Microsoft.Extensions.Logging;

	/// <summary>
	///     A static entry-point the a Xamarin Forms application.
	/// </summary>
	[PublicAPI]
	public static class XamarinApplicationHost
	{
		/// <summary>
		///     Creates the application.
		/// </summary>
		/// <typeparam name="TApplicationHost">The type of the host.</typeparam>
		/// <returns></returns>
		public static XamarinApplication BuildApplication<TApplicationHost>()
			where TApplicationHost : class, IXamarinApplicationHost, new()
		{
			IXamarinApplicationHost applicationHost = new TApplicationHost();
			return applicationHost.Build();
		}
	}

	/// <summary>
	///     An abstract base class for Xamarin Forms application hosts.
	/// </summary>
	/// <typeparam name="TStartupModule">The type of the startup module.</typeparam>
	/// <typeparam name="TApplication">The application type.</typeparam>
	[PublicAPI]
	public abstract class XamarinApplicationHost<TStartupModule, TApplication> : IXamarinApplicationHost
		where TStartupModule : class, IModule
		where TApplication : XamarinApplication
	{
		private IHostEnvironment environment;
		private ApplicationHostEvents events = new ApplicationHostEvents();
		private IHost host;
		private IHostBuilder hostBuilder;
		private ILogger logger;

		/// <inheritdoc />
		public Task StartAsync(CancellationToken cancellationToken = default)
		{
			return this.host.StartAsync(cancellationToken);
		}

		/// <inheritdoc />
		public Task StopAsync(CancellationToken cancellationToken = default)
		{
			return this.host.StopAsync(cancellationToken);
		}

		XamarinApplication IXamarinApplicationHost.Build()
		{
			TApplication application;

			try
			{
				// Start a stopwatch for measuring the startup time.
				Stopwatch stopwatch = Stopwatch.StartNew();

				// Configure the host events.
				this.ConfigureApplicationHostEvents(this.events);

				this.events.OnHostCreating();

				// Load the host environment.
				this.environment = this.CreateHostingEnvironment();

				// Create a logger as soon as possible to support early logging.
				this.logger = this.CreateLogger();

				this.logger.LogDebug("Host configuration starting.");

				// Create the host builder and configure it.
				this.hostBuilder = this.CreateHostBuilder()
					.ConfigureFoundationDefaults()
					.ConfigureHostBuilder(this.ConfigureHostBuilder)
					.ConfigureApplicationLoader<TStartupModule>(
						this.logger,
						this.ConfigureApplicationPlugins);

				// Build the host.
				this.host = this.BuildHost();

				// Initialize the application loader.
				IApplicationLoader applicationLoader = this.host.Services.GetRequiredService<IApplicationLoader>();
				applicationLoader.Initialize(new ApplicationLoaderInitializationContext(this.host.Services));

				IHostLifetime hostLifetime = this.host.Services.GetRequiredService<IHostLifetime>();
				this.logger.LogDebug("Running host using '{HostLifetime}'.", hostLifetime.GetType().Name);

				// Create the app instance.
				application = this.host.Services.GetRequiredService<TApplication>();
				application.Host = this.host;

				// Stop the stopwatch and log the startup time.
				stopwatch.Stop();
				this.logger.LogInformation("Host configured in {Duration} ms.", stopwatch.ElapsedMilliseconds);

				this.events.OnHostCreated();
			}
			catch(Exception ex)
			{
				this.events.OnHostCreationFailed(ex);
				this.logger?.LogCritical(ex, "Application terminated unexpectedly.");
				Trace.WriteLine(ex);
				Debug.WriteLine(ex);
				Console.Error.WriteLine(ex);

				throw;
			}
			finally
			{
				this.events.Dispose();
			}

			return application;
		}

		/// <inheritdoc />
		public IServiceProvider Services => this.host.Services;

		/// <summary>
		///     Creates a <see cref="ILoggerFactory" />.
		/// </summary>
		/// <param name="configuration">The application configuration.</param>
		/// <returns>The logger factory.</returns>
		protected virtual ILoggerFactory CreateBootstrapperLoggerFactory(IConfiguration configuration)
		{
			ILoggerFactory loggerFactory = LoggerFactory.Create(loggingBuilder =>
			{
				loggingBuilder.AddConfiguration(configuration.GetSection("Logging"));
				loggingBuilder.AddConsole();
			});

			return loggerFactory;
		}

		/// <summary>
		///     Configures the <see cref="IHostBuilder" /> instance.
		/// </summary>
		/// <param name="builder"></param>
		protected virtual void ConfigureHostBuilder(IHostBuilder builder)
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

		// See: HostBuilder.cs
		private IConfiguration BuildHostConfiguration()
		{
			IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
					.SetFileProvider(new EmbeddedFileProvider(typeof(TApplication).Assembly))
					.AddInMemoryCollection() // Make sure there's some default storage since there are no default providers.
					.AddEnvironmentVariables("DOTNET_")
				;

			IConfiguration configuration = configurationBuilder.Build();
			return configuration;
		}

		// See: HostBuilder.cs
		private IHostEnvironment CreateHostingEnvironment()
		{
			IConfiguration hostConfiguration = this.BuildHostConfiguration();

			HostingEnvironment hostingEnvironment = new HostingEnvironment
			{
				ApplicationName = hostConfiguration[HostDefaults.ApplicationKey],
				EnvironmentName = hostConfiguration[HostDefaults.EnvironmentKey] ?? Environments.Production,
				ContentRootPath = this.ResolveContentRootPath(hostConfiguration[HostDefaults.ContentRootKey], AppContext.BaseDirectory),
			};

			if(string.IsNullOrEmpty(hostingEnvironment.ApplicationName))
			{
				// Note GetEntryAssembly returns null for the net4x console test runner.
				hostingEnvironment.ApplicationName = Assembly.GetEntryAssembly()?.GetName().Name;
			}

			//hostingEnvironment.ContentRootFileProvider = new PhysicalFileProvider(hostingEnvironment.ContentRootPath);

			return hostingEnvironment;
		}

		// See: HostBuilder.cs
		private string ResolveContentRootPath(string contentRootPath, string basePath)
		{
			if(string.IsNullOrEmpty(contentRootPath))
			{
				return basePath;
			}

			if(Path.IsPathRooted(contentRootPath))
			{
				return contentRootPath;
			}

			return Path.Combine(Path.GetFullPath(basePath), contentRootPath);
		}

		private ILogger CreateLogger()
		{
			IConfiguration configuration = new ConfigurationBuilder()
				.SetFileProvider(new EmbeddedFileProvider(typeof(TApplication).Assembly))
				.AddInMemoryCollection() // Make sure there's some default storage since there are no default providers.
				.AddJsonFile("appsettings.json", true)
				.AddJsonFile($"appsettings.{this.environment.EnvironmentName}.json", true)
				.Build();

			ILoggerFactory loggerFactory = this.CreateBootstrapperLoggerFactory(configuration);
			return loggerFactory.CreateLogger(ApplicationHost.LoggerName);
		}

		private IHostBuilder CreateHostBuilder()
		{
			IHostBuilder builder = XamarinHost.CreateDefaultBuilder<TApplication>();
			return builder;
		}

		private IHost BuildHost()
		{
			// Build the host.
			return this.hostBuilder.Build();
		}
	}
}
