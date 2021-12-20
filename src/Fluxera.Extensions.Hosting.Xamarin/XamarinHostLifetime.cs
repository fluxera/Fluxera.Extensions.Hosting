namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;
	using Microsoft.Extensions.Logging.Abstractions;
	using Microsoft.Extensions.Options;

	public class XamarinHostLifetime : IHostLifetime, IDisposable
	{
		private CancellationTokenRegistration applicationStartedRegistration;

		public XamarinHostLifetime(
			IOptions<XamarinHostLifetimeOptions> options,
			IHostEnvironment environment,
			IHostApplicationLifetime applicationLifetime)
			: this(options, environment, applicationLifetime, NullLoggerFactory.Instance)
		{
		}

		public XamarinHostLifetime(
			IOptions<XamarinHostLifetimeOptions> options,
			IHostEnvironment environment,
			IHostApplicationLifetime applicationLifetime,
			ILoggerFactory loggerFactory)
		{
			this.Options = options?.Value ?? throw new ArgumentNullException(nameof(options));
			this.Environment = environment ?? throw new ArgumentNullException(nameof(environment));
			this.Lifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
			this.Logger = loggerFactory.CreateLogger("Microsoft.Extensions.Hosting.Host");
		}

		private IHostEnvironment Environment { get; }

		private IHostApplicationLifetime Lifetime { get; }

		private ILogger Logger { get; }

		private XamarinHostLifetimeOptions Options { get; }

		public void Dispose()
		{
			this.applicationStartedRegistration.Dispose();
		}

		public Task WaitForStartAsync(CancellationToken cancellationToken)
		{
			if(!this.Options.SuppressStatusMessages)
			{
				this.applicationStartedRegistration = this.Lifetime.ApplicationStarted.Register(state =>
					{
						((XamarinHostLifetime)state).OnApplicationStarted();
					},
					this);
			}

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		private void OnApplicationStarted()
		{
			this.Logger.LogInformation("Application started.");
			this.Logger.LogInformation("Hosting environment: {envName}", this.Environment.EnvironmentName);
		}
	}
}
