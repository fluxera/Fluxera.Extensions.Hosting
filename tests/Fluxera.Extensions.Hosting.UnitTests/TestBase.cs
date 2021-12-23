namespace Fluxera.Extensions.Hosting.UnitTests
{
	using System;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;

	[PublicAPI]
	public abstract class TestBase
	{
		protected static IServiceProvider BuildServiceProvider(Action<IServiceCollection> configure)
		{
			IServiceCollection services = new ServiceCollection();

			services.AddLogging(builder =>
			{
				builder.SetMinimumLevel(LogLevel.Trace);
				builder.AddConsole();
			});

			configure(services);
			return services.BuildServiceProvider();
		}

		protected static ILogger CreateBootstrapperLogger()
		{
			ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
			{
				builder.SetMinimumLevel(LogLevel.Trace);
				builder.AddConsole();
			});

			return loggerFactory.CreateLogger(ApplicationHost.LoggerName);
		}
	}
}
