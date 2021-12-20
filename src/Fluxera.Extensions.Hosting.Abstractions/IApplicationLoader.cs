namespace Fluxera.Extensions.Hosting
{
	using System;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;

	[PublicAPI]
	public interface IApplicationLoader : IModuleContainer, IDisposable
	{
		/// <summary>
		///     Type of the startup (entry) module of the application.
		/// </summary>
		Type StartupModuleType { get; }

		/// <summary>
		///     List of services registered to this application.
		///     Can not add new services to this collection after application initialize.
		/// </summary>
		IServiceCollection Services { get; }

		/// <summary>
		///     The configuration instance of the application.
		/// </summary>
		IConfiguration Configuration { get; }

		/// <summary>
		///     Reference to the plugin sources to use when loading modules.
		/// </summary>
		IPluginSourceList PluginSources { get; }

		/// <summary>
		///     Reference to the root service provider used by the application.
		///     This can not be used before initialize the application.
		/// </summary>
		IServiceProvider ServiceProvider { get; }

		/// <summary>
		///     Initializes the application using the provided host instance.
		/// </summary>
		/// <param name="context"></param>
		void Initialize(IApplicationLoaderInitializationContext context);

		/// <summary>
		///     Used to gracefully shutdown the application and all modules.
		/// </summary>
		void Shutdown();
	}
}
