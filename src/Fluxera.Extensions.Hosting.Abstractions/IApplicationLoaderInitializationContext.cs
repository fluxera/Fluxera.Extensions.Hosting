namespace Fluxera.Extensions.Hosting
{
	using System;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a context class that is passed in to the Initialize
	///     method of the application loader.
	/// </summary>
	/// <remarks>
	///     Some host may need a special implementation, but n most cases the
	///     default implementation is sufficient.
	/// </remarks>
	[PublicAPI]
	public interface IApplicationLoaderInitializationContext
	{
		/// <summary>
		///     Gets the service provider of the application.
		/// </summary>
		IServiceProvider ServiceProvider { get; }
	}
}
