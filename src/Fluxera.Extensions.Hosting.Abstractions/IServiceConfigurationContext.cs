namespace Fluxera.Extensions.Hosting
{
	using System.Collections.Generic;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;

	[PublicAPI]
	public interface IServiceConfigurationContext
	{
		IServiceCollection Services { get; }

		IConfiguration Configuration { get; }

		IHostEnvironment Environment { get; }

		ILogger Logger { get; }

		IDictionary<string, object> Items { get; }

		object? this[string key] { get; set; }
	}
}
