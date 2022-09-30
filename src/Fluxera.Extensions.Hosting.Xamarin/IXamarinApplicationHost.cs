namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for an Xamarin Forms application host.
	/// </summary>
	[PublicAPI]
	[Obsolete("The hosting library for Xamarin.Forms will be remove in the 7.0 release.")]
	public interface IXamarinApplicationHost
	{
		/// <summary>
		///     Gets the service collection.
		/// </summary>
		IServiceProvider Services { get; }

		/// <summary>
		///     Starts the application host.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		Task StartAsync(CancellationToken cancellationToken = default);

		/// <summary>
		///     Tries to gracefully stop the application.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		Task StopAsync(CancellationToken cancellationToken = default);

		/// <summary>
		///     Builds the application instance.
		/// </summary>
		/// <returns></returns>
		internal XamarinApplication Build();
	}
}
