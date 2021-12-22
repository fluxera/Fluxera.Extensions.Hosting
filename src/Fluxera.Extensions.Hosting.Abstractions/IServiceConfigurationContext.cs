namespace Fluxera.Extensions.Hosting
{
	using System.Collections.Generic;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;

	/// <summary>
	///     A contract for a context type that is available during the service
	///     configuration of modules. One service context instance is passed
	///     down the module list.
	/// </summary>
	[PublicAPI]
	public interface IServiceConfigurationContext
	{
		/// <summary>
		///     Gets the service collection.
		/// </summary>
		IServiceCollection Services { get; }

		/// <summary>
		///     Gets the application configuration.
		/// </summary>
		IConfiguration Configuration { get; }

		/// <summary>
		///     Gets the environment to application runs under.
		/// </summary>
		IHostEnvironment Environment { get; }

		/// <summary>
		///     Gets a logger.
		/// </summary>
		ILogger Logger { get; }

		/// <summary>
		///     Gets additional items that can be passed down the module service configuration pipeline.
		///     This items are shared between modules during the service configuration phase.
		/// </summary>
		IDictionary<string, object> Items { get; }

		/// <summary>
		///     Gets or sets named objects that can be stored during the service configuration phase
		///     and shared between modules. This is a shortcut usage of the <see cref="Items" />
		///     dictionary. Returns null if given key is not found in the <see cref="Items" />
		///     dictionary.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The stored object or <c>null</c>.</returns>
		object? this[string key] { get; set; }
	}
}
