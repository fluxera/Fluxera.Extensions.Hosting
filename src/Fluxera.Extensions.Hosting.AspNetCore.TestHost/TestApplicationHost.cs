namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Diagnostics;
	using System.Threading.Tasks;
	using Fluxera.Extensions.Hosting.Modules;
	using JetBrains.Annotations;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.TestHost;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;

	/// <summary>
	///		A static entry-point for modular test host applications.
	/// </summary>
	[PublicAPI]
	public static class TestApplicationHost
	{
		/// <summary>
		///     Creates and runs the test host.
		/// </summary>
		/// <typeparam name="TApplicationHost">The test host type.</typeparam>
		/// <param name="args">The command line arguments.</param>
		/// <returns>A task running the application host.</returns>
		public static async Task<TestServer> RunAsync<TApplicationHost>(string[] args = null)
			where TApplicationHost : class, ITestApplicationHost, new()
		{
			args ??= Array.Empty<string>();

			ITestApplicationHost applicationHost = new TApplicationHost();
			await applicationHost.RunAsync(args);
			return applicationHost.Server;
		}
	}

	/// <summary>
	///		An abstract base class for test host applications.
	/// </summary>
	/// <typeparam name="TStartupModule"></typeparam>
	[PublicAPI]
	public abstract class TestApplicationHost<TStartupModule> : WebApplicationHost<TStartupModule>, ITestApplicationHost
		where TStartupModule : class, IModule
	{
		/// <inheritdoc />
		public TestServer Server { get; private set; }

		/// <inheritdoc />
		public new Task RunAsync(string[] args)
		{
			// Create a logger as soon as possible to support early logging.
			ILogger logger = this.CreateLogger();

			// Create the host builder and configure it.
			IWebHostBuilder hostBuilder = new WebHostBuilder()
				.ConfigureFoundationDefaults()
				.ConfigureHostBuilder(this.ConfigureHostBuilder)
				.ConfigureApplicationLoader<TStartupModule>(
					logger,
					this.ConfigureApplicationPlugins,
					this.ApplicationLoaderBuilder)
				.Configure(app =>
				{
					IApplicationLoader applicationLoader = app.ApplicationServices.GetRequiredService<IApplicationLoader>();
					applicationLoader.Initialize(new WebApplicationLoaderInitializationContext(app));
				});

			this.Server = new TestServer(hostBuilder);

			return Task.CompletedTask;
		}

		/// <inheritdoc />
		protected sealed override IHost BuildHost()
		{
			throw new UnreachableException();
		}

		/// <inheritdoc />
		protected sealed override void ConfigureHostBuilder(IHostBuilder builder)
		{
			throw new UnreachableException();
		}

		/// <summary>
		///     Configures the <see cref="IHostBuilder" /> instance.
		/// </summary>
		/// <param name="builder"></param>
		protected virtual void ConfigureHostBuilder(IWebHostBuilder builder)
		{
		}

		/// <inheritdoc />
		protected sealed override IHostBuilder CreateHostBuilder()
		{
			throw new UnreachableException();
		}

		/// <inheritdoc />
		protected sealed override void InitializeApplicationLoader(IHost host)
		{
			throw new UnreachableException();
		}

		private ILogger CreateLogger()
		{
			IConfiguration configuration = new ConfigurationBuilder()
				.AddInMemoryCollection() // Make sure there's some default storage since there are no default providers.
				.AddJsonFile("appsettings.json", true)
				.Build();

			ILoggerFactory loggerFactory = this.CreateBootstrapperLoggerFactory(configuration);
			return loggerFactory.CreateLogger(ApplicationHost.LoggerName);
		}
	}
}
