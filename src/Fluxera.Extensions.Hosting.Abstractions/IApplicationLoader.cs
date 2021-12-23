namespace Fluxera.Extensions.Hosting
{
	using System;
	using Fluxera.Extensions.Hosting.Modules;
	using Fluxera.Extensions.Hosting.Plugins;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;

	/// <summary>
	///     A contract for a modular application loader which configures and loads
	///     the configured modules and plugins.
	/// </summary>
	[PublicAPI]
	public interface IApplicationLoader : IModuleContainer, IDisposable
	{
		/// <summary>
		///     Gets the type of the startup module.
		/// </summary>
		Type StartupModuleType { get; }

		/// <summary>
		///     Gets the service collection.
		/// </summary>
		IServiceCollection Services { get; }

		/// <summary>
		///     Gets the configuration of the application.
		/// </summary>
		IConfiguration Configuration { get; }

		/// <summary>
		///     Gets the plugin sources to use when loading modules.
		/// </summary>
		IPluginSourceList PluginSources { get; }

		/// <summary>
		///     Gets the service provider.
		/// </summary>
		IServiceProvider ServiceProvider { get; }

		/// <summary>
		///     Initializes the application using the given context.
		/// </summary>
		/// <remarks>
		///     See: <see cref="IApplicationLoaderInitializationContext" />
		/// </remarks>
		/// <param name="context"></param>
		void Initialize(IApplicationLoaderInitializationContext context);

		/// <summary>
		///     Gracefully shuts down the application.
		/// </summary>
		void Shutdown();
	}
}
