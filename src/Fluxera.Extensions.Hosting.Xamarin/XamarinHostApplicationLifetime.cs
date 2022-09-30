namespace Fluxera.Extensions.Hosting
{
	using System;
	using Microsoft.Extensions.Hosting.Internal;
	using Microsoft.Extensions.Logging;

	/// <summary>
	///     A service that allows to perform execution of custom action during application sleep/resume cycle.
	/// </summary>
	[Obsolete("The hosting library for Xamarin.Forms will be remove in the 7.0 release.")]
	public sealed class XamarinHostApplicationLifetime : ApplicationLifetime, IXamarinHostApplicationLifetime
	{
		private readonly ILogger logger;

		/// <summary>
		///     Creates a new instance of the <see cref="XamarinHostApplicationLifetime" /> type.
		/// </summary>
		/// <param name="logger"></param>
		public XamarinHostApplicationLifetime(ILogger<ApplicationLifetime> logger)
			: base(logger)
		{
			this.logger = logger;
			this.Sleeping = new LifecycleRegister();
			this.Resuming = new LifecycleRegister();
		}

		/// <inheritdoc />
		public ILifecycleRegister Sleeping { get; }

		/// <inheritdoc />
		public ILifecycleRegister Resuming { get; }

		/// <inheritdoc />
		void IXamarinHostApplicationLifetime.NotifySleeping()
		{
			try
			{
				this.Sleeping.Notify();
			}
			catch(Exception ex)
			{
				this.logger.LogCriticalEx("An error occurred while pausing the application.", ex);
			}
		}

		/// <inheritdoc />
		void IXamarinHostApplicationLifetime.NotifyResuming()
		{
			try
			{
				this.Resuming.Notify();
			}
			catch(Exception ex)
			{
				this.logger.LogCriticalEx("An error occurred while resuming the application.", ex);
			}
		}
	}
}
