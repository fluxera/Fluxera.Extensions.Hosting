namespace Fluxera.Extensions.Hosting
{
	using System;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.Hosting;

	/// <summary>
	///     A contract for a context type that is available during the application
	///     initialization of modules. One service context instance is passed
	///     down the module list.
	/// </summary>
	[PublicAPI]
	public interface IApplicationInitializationContext : ILoggingContext<IServiceProvider>
	{
		/// <summary>
		///     Gets the service provider.
		/// </summary>
		IServiceProvider ServiceProvider { get; }

		/// <summary>
		///     Gets the application configuration.
		/// </summary>
		IConfiguration Configuration { get; }

		/// <summary>
		///     Gets the environment the host runs under.
		/// </summary>
		IHostEnvironment Environment { get; }
	}
}
