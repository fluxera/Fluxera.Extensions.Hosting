namespace Fluxera.Extensions.Hosting
{
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.Extensions.Hosting;

	internal sealed class WebAssemblyLifetime : IHostLifetime
	{
		/// <inheritdoc />
		public Task WaitForStartAsync(CancellationToken cancellationToken)
		{
			// Wasm applications start immediately.
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public Task StopAsync(CancellationToken cancellationToken)
		{
			// There's nothing to do here.
			return Task.CompletedTask;
		}
	}
}
