namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;
	using Microsoft.Extensions.Logging.Abstractions;
	using Microsoft.Extensions.Options;

	/// <summary>
	///     A <see cref="IHostLifetime" /> implementation for Xamarin Forms applications.
	/// </summary>
	[Obsolete("The hosting library for Xamarin.Forms will be remove in the 7.0 release.")]
	public sealed class XamarinHostLifetime : IHostLifetime, IDisposable
	{
		private CancellationTokenRegistration applicationStartedRegistration;

		/// <summary>
		///     Creates a new instance of the <see cref="XamarinHostLifetime" /> type.
		/// </summary>
		/// <param name="options"></param>
		/// <param name="environment"></param>
		/// <param name="applicationLifetime"></param>
		public XamarinHostLifetime(
			IOptions<XamarinHostLifetimeOptions> options,
			IHostEnvironment environment,
			IHostApplicationLifetime applicationLifetime)
			: this(options, environment, applicationLifetime, NullLoggerFactory.Instance)
		{
		}

		/// <summary>
		///     Creates a new instance of the <see cref="XamarinHostLifetime" /> type.
		/// </summary>
		/// <param name="options"></param>
		/// <param name="environment"></param>
		/// <param name="applicationLifetime"></param>
		/// <param name="loggerFactory"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public XamarinHostLifetime(
			IOptions<XamarinHostLifetimeOptions> options,
			IHostEnvironment environment,
			IHostApplicationLifetime applicationLifetime,
			ILoggerFactory loggerFactory)
		{
			this.Options = options.Value ?? throw new ArgumentNullException(nameof(options));
			this.Environment = environment ?? throw new ArgumentNullException(nameof(environment));
			this.Lifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
			this.Logger = loggerFactory.CreateLogger("Microsoft.Extensions.Hosting.Host");
		}

		private IHostEnvironment Environment { get; }

		private IHostApplicationLifetime Lifetime { get; }

		private ILogger Logger { get; }

		private XamarinHostLifetimeOptions Options { get; }

		/// <inheritdoc />
		public void Dispose()
		{
			this.applicationStartedRegistration.Dispose();
		}

		/// <inheritdoc />
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

		/// <inheritdoc />
		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		private void OnApplicationStarted()
		{
			this.Logger.LogInformation("Application started.");
			this.Logger.LogInformation("Hosting environment: {Environment}", this.Environment.EnvironmentName);
		}
	}
}
