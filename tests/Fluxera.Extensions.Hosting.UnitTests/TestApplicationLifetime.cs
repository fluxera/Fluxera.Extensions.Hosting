namespace Fluxera.Extensions.Hosting.UnitTests
{
	using System.Threading;
	using Microsoft.Extensions.Hosting;

	public class TestApplicationLifetime : IHostApplicationLifetime
	{
		/// <inheritdoc />
		public void StopApplication()
		{
		}

		/// <inheritdoc />
		public CancellationToken ApplicationStarted { get; }

		/// <inheritdoc />
		public CancellationToken ApplicationStopping { get; }

		/// <inheritdoc />
		public CancellationToken ApplicationStopped { get; }
	}
}
