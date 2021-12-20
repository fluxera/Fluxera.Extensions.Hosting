namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;

	/// <summary>
	///     Extends the <see cref="IHost" />.
	/// </summary>
	[PublicAPI]
	public static class HostExtensions
	{
		/// <summary>
		///     Puts the host to sleep synchronously.
		/// </summary>
		/// <param name="host">The <see cref="IHost" /> that is being extended.</param>
		public static void Sleep(this IHost host)
		{
			host.SleepAsync().GetAwaiter().GetResult();
		}

		/// <summary>
		///     Signals that the <see cref="IHost" /> will be sleeping.
		/// </summary>
		/// <param name="host">The <see cref="IHost" /> that is being extended.</param>
		public static async Task SleepAsync(this IHost host, CancellationToken cancellationToken = default)
		{
			IEnumerable<IHostedService>? hostedServices = host.Services.GetService<IEnumerable<IHostedService>>();

			IList<Exception> exceptions = new List<Exception>();
			if(hostedServices != null)
			{
				foreach(IHostedService? hostedService in hostedServices.Reverse())
				{
					cancellationToken.ThrowIfCancellationRequested();
					try
					{
						// Fire IXamarinHostedService.Sleep
						if(hostedService is IXamarinHostedService service)
						{
							await service.SleepAsync(cancellationToken).ConfigureAwait(false);
						}
					}
					catch(Exception ex)
					{
						exceptions.Add(ex);
					}
				}
			}

			XamarinHostApplicationLifetime? lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>() as XamarinHostApplicationLifetime;
			lifetime?.NotifySleeping();

			ILogger<IHost> logger = host.Services.GetRequiredService<ILogger<IHost>>();

			if(exceptions.Count > 0)
			{
				AggregateException ex = new AggregateException("One or more hosted services failed to stop.", exceptions);
				logger.StoppedWithException(ex);
				throw ex;
			}

			logger.Sleeping();
		}

		/// <summary>
		///     Resumes the host synchronously.
		/// </summary>
		/// <param name="host">The <see cref="IHost" /> that is being extended.</param>
		public static void Resume(this IHost host)
		{
			host.ResumeAsync().GetAwaiter().GetResult();
		}

		/// <summary>
		///     Signals that the <see cref="IHost" /> will be resuming.
		/// </summary>
		/// <param name="host">The <see cref="IHost" /> that is being extended.</param>
		public static async Task ResumeAsync(this IHost host, CancellationToken cancellationToken = default)
		{
			IEnumerable<IHostedService>? hostedServices = host.Services.GetService<IEnumerable<IHostedService>>();
			foreach(IHostedService? hostedService in hostedServices)
			{
				// Fire IXamarinHostedService.Sleep
				if(hostedService is IXamarinHostedService service)
				{
					await service.ResumeAsync(cancellationToken).ConfigureAwait(false);
				}
			}

			XamarinHostApplicationLifetime? lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>() as XamarinHostApplicationLifetime;
			lifetime?.NotifyResuming();

			ILogger<IHost> logger = host.Services.GetRequiredService<ILogger<IHost>>();
			logger.Resuming();
		}
	}
}
