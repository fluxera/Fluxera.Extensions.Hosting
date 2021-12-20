namespace Fluxera.Extensions.Hosting
{
	using System;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class HostInitializationFailedEventArgs : EventArgs
	{
		public HostInitializationFailedEventArgs(Exception exception)
		{
			this.Exception = exception;
		}

		public Exception Exception { get; }
	}
}
