namespace Fluxera.Extensions.Hosting.Plugins
{
	using JetBrains.Annotations;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;

	/// <summary>
	///     A contract for a context type that is given to the plugin module configuration.
	/// </summary>
	[PublicAPI]
	public interface IPluginConfigurationContext
	{
		/// <summary>
		///     The list of plugin sources. One may add additional sources to configure
		///     the usage of plugins. use the extensions available for <see cref="IPluginConfigurationContext" />.
		/// </summary>
		IPluginSourceList PluginSources { get; }

		/// <summary>
		///     Gets the configuration of the application.
		/// </summary>
		IConfiguration Configuration { get; }

		/// <summary>
		///     Gets the environment the application runs under.
		/// </summary>
		IHostEnvironment Environment { get; }

		/// <summary>
		///     Gets a logger.
		/// </summary>
		ILogger Logger { get; }
	}
}
