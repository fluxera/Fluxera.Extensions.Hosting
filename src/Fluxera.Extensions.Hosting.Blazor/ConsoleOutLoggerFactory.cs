namespace Fluxera.Extensions.Hosting
{
	using System;
	using Microsoft.Extensions.Logging;

	internal sealed class ConsoleOutLoggerFactory : ILoggerFactory
	{
		private readonly ILoggerProvider loggerProvider;

		public ConsoleOutLoggerFactory()
		{
			this.loggerProvider = new ConsoleOutLoggerProvider();
		}

		/// <inheritdoc />
		public ILogger CreateLogger(string categoryName)
		{
			return this.loggerProvider.CreateLogger(categoryName);
		}

		/// <inheritdoc />
		public void AddProvider(ILoggerProvider provider)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public void Dispose()
		{
			this.loggerProvider.Dispose();
		}
	}
}
