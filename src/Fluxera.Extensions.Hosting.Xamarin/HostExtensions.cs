namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Utilities;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;

	/// <summary>
	///     Extension methods on the <see cref="IHost" /> type.
	/// </summary>
	[PublicAPI]
	public static class HostExtensions
	{
		/// <summary>
		///     Puts the host to sleep synchronously.
		/// </summary>
		/// <param name="host">The host.</param>
		public static void Sleep(this IHost host)
		{
			AsyncHelper.RunSync(() => host.SleepAsync());
		}

		/// <summary>
		///     Signals that the <see cref="IHost" /> will be sleeping.
		/// </summary>
		/// <param name="host">The host.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		public static async Task SleepAsync(this IHost host, CancellationToken cancellationToken = default)
		{
			IEnumerable<IHostedService> hostedServices = host.Services.GetServices<IHostedService>();

			IList<Exception> exceptions = new List<Exception>();
			foreach(IHostedService? hostedService in hostedServices.Reverse())
			{
				cancellationToken.ThrowIfCancellationRequested();
				try
				{
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
		/// <param name="host">The host.</param>
		public static void Resume(this IHost host)
		{
			AsyncHelper.RunSync(() => host.ResumeAsync());
		}

		/// <summary>
		///     Signals that the <see cref="IHost" /> will be resuming.
		/// </summary>
		/// <param name="host">The host.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		public static async Task ResumeAsync(this IHost host, CancellationToken cancellationToken = default)
		{
			IEnumerable<IHostedService> hostedServices = host.Services.GetServices<IHostedService>();
			foreach(IHostedService hostedService in hostedServices)
			{
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
