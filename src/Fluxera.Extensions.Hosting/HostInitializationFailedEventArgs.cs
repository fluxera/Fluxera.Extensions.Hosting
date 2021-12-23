namespace Fluxera.Extensions.Hosting
{
	using System;
	using JetBrains.Annotations;

	/// <summary>
	///     Event args that provide the exception that occurred when the host initialization failed.
	/// </summary>
	[PublicAPI]
	public sealed class HostInitializationFailedEventArgs : EventArgs
	{
		/// <summary>
		///     Creates a new instance of the <see cref="HostInitializationFailedEventArgs" /> type.
		/// </summary>
		/// <param name="exception">The exception.</param>
		public HostInitializationFailedEventArgs(Exception exception)
		{
			this.Exception = exception;
		}

		/// <summary>
		///     Gets the exception.
		/// </summary>
		public Exception Exception { get; }
	}
}
