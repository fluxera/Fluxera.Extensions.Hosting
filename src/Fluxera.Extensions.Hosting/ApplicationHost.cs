namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Reflection;
	using System.Threading.Tasks;
	using Fluxera.Extensions.Hosting.Modules;
	using Fluxera.Extensions.Hosting.Plugins;
	using Fluxera.Guards;
	using Fluxera.Utilities.Extensions;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.FileProviders;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Hosting.Internal;
	using Microsoft.Extensions.Logging;

	/// <summary>
	///     A static entry-point for modular host-based applications.
	/// </summary>
	[PublicAPI]
	public static class ApplicationHost
	{
		/// <summary>
		///     The logger name.
		/// </summary>
		public const string LoggerName = "Fluxera.Extensions.Hosting.ApplicationHost";

		/// <summary>
		///     Creates and runs the application host.
		/// </summary>
		/// <typeparam name="TApplicationHost">The host type.</typeparam>
		/// <param name="args">The command line arguments.</param>
		/// <returns>A task running the application host.</returns>
		public static Task RunAsync<TApplicationHost>(string[] args)
			where TApplicationHost : class, IApplicationHost, new()
		{
			IApplicationHost applicationHost = new TApplicationHost();
			return applicationHost.RunAsync(args);
		}

		/// <summary>
		///     Creates and runs the application host.
		/// </summary>
		/// <typeparam name="TApplicationHost">The host type.</typeparam>
		/// <param name="args">The command line arguments.</param>
		/// <returns>A task running the application host.</returns>
		public static void Run<TApplicationHost>(string[] args)
			where TApplicationHost : class, IApplicationHost, new()
		{
			IApplicationHost applicationHost = new TApplicationHost();
			applicationHost.RunAsync(args).GetAwaiter().GetResult();
		}
	}


	/// <summary>
	///     A static entry-point for modular host-based applications.
	/// </summary>
	[PublicAPI]
	public abstract class ApplicationHost<TStartupModule> : IApplicationHost
		where TStartupModule : class, IModule
	{
		private IHostEnvironment environment = null!;
		private ApplicationHostEvents events = new ApplicationHostEvents();
		private IHostBuilder hostBuilder = null!;
		private ILogger logger = null!;

		/// <summary>
		///     Gets the command line arguments of the application.
		/// </summary>
		protected string[] CommandLineArgs { get; private set; } = null!;

		/// <summary>
		///     Gets the optional allocation loader builder function.
		/// </summary>
		protected virtual ApplicationLoaderBuilderFunc ApplicationLoaderBuilder { get; }

		/// <summary>
		///     Gets the prefixes of environment variables that should be loaded into the
		///     host initialization configuration.
		/// </summary>
		protected virtual IEnumerable<string> HostConfigurationEnvironmentVariablesPrefixes
		{
			get
			{
				yield return "FLUXERA_";
				yield return "DOTNET_";
			}
		}

		/// <inheritdoc />
		public async Task RunAsync(string[] args)
		{
			Guard.Against.Null(args, nameof(args));

			try
			{
				// Start a stopwatch for measuring the startup time.
				Stopwatch stopwatch = Stopwatch.StartNew();

				this.CommandLineArgs = args;

				// Configure the host events.
				this.ConfigureApplicationHostEvents(this.events);

				this.events.OnHostCreating();

				// Load the host environment.
				this.environment = this.CreateHostingEnvironment(args);

				// Create a logger as soon as possible to support early logging.
				this.logger = this.CreateLogger();

				this.logger.LogDebug("Host configuration starting.");

				// Create the host builder and configure it.
				this.hostBuilder = this.CreateHostBuilder()
					.ConfigureFoundationDefaults()
					.ConfigureHostBuilder(this.ConfigureHostBuilder)
					.ConfigureApplicationLoader<TStartupModule>(
						this.logger,
						this.ConfigureApplicationPlugins,
						this.ApplicationLoaderBuilder);

				// Build the host.
				IHost host = this.BuildHost();

				// Initialize the application loader.
				this.InitializeApplicationLoader(host);

				IHostLifetime hostLifetime = host.Services.GetRequiredService<IHostLifetime>();
				this.logger.LogDebug("Running host using '{HostLifetime}'.", hostLifetime.GetType().Name);

				// Stop the stopwatch and log the startup time.
				stopwatch.Stop();
				this.logger.LogInformation("Host configured in {Duration} ms.", stopwatch.ElapsedMilliseconds);

				this.events.OnHostCreated();

				// Run the host.
				await host.RunAsync();
			}
			catch(Exception ex)
			{
				this.events.OnHostCreationFailed(ex);
				this.logger?.LogCritical(ex, "Application terminated unexpectedly.");
				Trace.WriteLine(ex);
				Debug.WriteLine(ex);
				Console.Error.WriteLine(ex);
			}
			finally
			{
				this.events.Dispose();
			}
		}

		/// <summary>
		///     Creates the <see cref="IHostBuilder" /> instance to use.
		/// </summary>
		/// <returns></returns>
		protected virtual IHostBuilder CreateHostBuilder()
		{
			// Create the host builder.
			return Host.CreateDefaultBuilder(this.CommandLineArgs);
		}

		/// <summary>
		///     Builds the <see cref="IHost" /> instance.
		/// </summary>
		/// <returns></returns>
		protected virtual IHost BuildHost()
		{
			// Build the host.
			return this.hostBuilder.Build();
		}

		/// <summary>
		///     Initializes the <see cref="IApplicationLoader" /> from the given <see cref="IHost" />.
		/// </summary>
		/// <param name="host"></param>
		protected virtual void InitializeApplicationLoader(IHost host)
		{
			IApplicationLoader applicationLoader = host.Services.GetRequiredService<IApplicationLoader>();
			applicationLoader.Initialize(new ApplicationLoaderInitializationContext(host.Services));
		}

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
		private IConfiguration BuildHostConfiguration(string[] args)
		{
			IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
				.AddInMemoryCollection() // Make sure there's some default storage since there are no default providers.
				.AddCommandLine(args);

			foreach(string prefix in this.HostConfigurationEnvironmentVariablesPrefixes)
			{
				if(prefix.IsNotNullOrWhiteSpace())
				{
					configurationBuilder.AddEnvironmentVariables(prefix);
				}
			}

			IConfiguration configuration = configurationBuilder.Build();
			return configuration;
		}

		// See: HostBuilder.cs
		private IHostEnvironment CreateHostingEnvironment(string[] args)
		{
			IConfiguration hostConfiguration = this.BuildHostConfiguration(args);

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

			hostingEnvironment.ContentRootFileProvider = new PhysicalFileProvider(hostingEnvironment.ContentRootPath);

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
				.AddInMemoryCollection() // Make sure there's some default storage since there are no default providers.
				.AddJsonFile("appsettings.json", true)
				.AddJsonFile($"appsettings.{this.environment.EnvironmentName}.json", true)
				.Build();

			ILoggerFactory loggerFactory = this.CreateBootstrapperLoggerFactory(configuration);
			return loggerFactory.CreateLogger(ApplicationHost.LoggerName);
		}
	}
}
