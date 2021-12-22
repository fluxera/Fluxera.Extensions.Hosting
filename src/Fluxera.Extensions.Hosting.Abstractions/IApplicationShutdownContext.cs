﻿namespace Fluxera.Extensions.Hosting
{
	using System;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;

	/// <summary>
	///     A contract for a context type that is available during the application
	///     shutdown of modules. One service context instance is passed down the module list.
	/// </summary>
	[PublicAPI]
	public interface IApplicationShutdownContext
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
		///     Gets the environment the host run under.
		/// </summary>
		IHostEnvironment Environment { get; }

		/// <summary>
		///     Gets a logger.
		/// </summary>
		ILogger Logger { get; set; }
	}
}
