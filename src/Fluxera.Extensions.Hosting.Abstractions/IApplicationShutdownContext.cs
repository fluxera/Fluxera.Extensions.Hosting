namespace Fluxera.Extensions.Hosting
{
	using System;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;

	[PublicAPI]
	public interface IApplicationShutdownContext
	{
		IServiceProvider ServiceProvider { get; }

		IConfiguration Configuration { get; }

		IHostEnvironment Environment { get; }

		ILogger Logger { get; set; }
	}
}
