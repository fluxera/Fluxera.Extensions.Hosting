namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;

	[PublicAPI]
	public interface IPluginConfigurationContext
	{
		IPluginSourceList PluginSources { get; }

		IConfiguration Configuration { get; }

		IHostEnvironment Environment { get; }

		ILogger Logger { get; }
	}
}
