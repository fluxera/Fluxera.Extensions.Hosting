namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;
	using Microsoft.Extensions.Logging;

	/// <summary>
	///     A contract for a context that provides logging capabilities.
	/// </summary>
	[PublicAPI]
	public interface ILoggingContext<out T> : ILoggingContext
		where T : class
	{
		/// <summary>
		///     Gets the context data, f.e. IServiceCollection.
		/// </summary>
		internal T LogContextData { get; }
	}

	/// <summary>
	///     A contract for a context that provides logging capabilities.
	/// </summary>
	[PublicAPI]
	public interface ILoggingContext
	{
		/// <summary>
		///     Gets a logger.
		/// </summary>
		ILogger Logger { get; }
	}
}
