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
	[Obsolete("The hosting library for Xamarin.Forms will be remove in the 7.0 release.")]
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
		///     Notifies that the <see cref="IHost" /> will sleep.
		/// </summary>
		/// <param name="host">The host.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		public static async Task SleepAsync(this IHost host, CancellationToken cancellationToken = default)
		{
			IEnumerable<IHostedService> hostedServices = host.Services.GetServices<IHostedService>();

			IList<Exception> exceptions = new List<Exception>();
			foreach(IHostedService hostedService in hostedServices.Reverse())
			{
				cancellationToken.ThrowIfCancellationRequested();
				try
				{
					await hostedService.StopAsync(cancellationToken).ConfigureAwait(false);
				}
				catch(Exception ex)
				{
					exceptions.Add(ex);
				}
			}

			IXamarinHostApplicationLifetime lifetime = host.Services.GetRequiredService<IXamarinHostApplicationLifetime>();
			lifetime.NotifySleeping();

			if(exceptions.Count > 0)
			{
				AggregateException ex = new AggregateException("One or more hosted services failed to stop.", exceptions);

				ILogger<IHost> logger = host.Services.GetRequiredService<ILogger<IHost>>();
				logger.LogCritical(ex, "An error occurred while stopping hosted services.");

				throw ex;
			}
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
		///     Notifies that the <see cref="IHost" /> will resume.
		/// </summary>
		/// <param name="host">The host.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		public static async Task ResumeAsync(this IHost host, CancellationToken cancellationToken = default)
		{
			IEnumerable<IHostedService> hostedServices = host.Services.GetServices<IHostedService>();
			foreach(IHostedService hostedService in hostedServices)
			{
				await hostedService.StartAsync(cancellationToken).ConfigureAwait(false);
			}

			IXamarinHostApplicationLifetime lifetime = host.Services.GetRequiredService<IXamarinHostApplicationLifetime>();
			lifetime.NotifyResuming();
		}
	}
}
