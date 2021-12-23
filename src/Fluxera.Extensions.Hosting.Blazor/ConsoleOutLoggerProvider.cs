namespace Fluxera.Extensions.Hosting
{
	using Microsoft.Extensions.Logging;

	internal sealed class ConsoleOutLoggerProvider : ILoggerProvider
	{
		/// <inheritdoc />
		public ILogger CreateLogger(string categoryName)
		{
			return new ConsoleOutLogger(categoryName);
		}

		/// <inheritdoc />
		public void Dispose()
		{
		}
	}
}
