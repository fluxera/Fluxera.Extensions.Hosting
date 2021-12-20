namespace Fluxera.Extensions.Hosting
{
	using System;

	public sealed class ApplicationHostEvents : IDisposable
	{
		public event EventHandler? HostCreating;
		public event EventHandler? HostCreated;
		public event EventHandler<HostInitializationFailedEventArgs>? HostCreationFailed;

		public void OnHostCreating()
		{
			this.HostCreating?.Invoke(null, EventArgs.Empty);
		}

		public void OnHostCreated()
		{
			this.HostCreated?.Invoke(null, EventArgs.Empty);
		}

		public void OnHostCreationFailed(Exception exception)
		{
			this.HostCreationFailed?.Invoke(null, new HostInitializationFailedEventArgs(exception));
		}

		/// <inheritdoc />
		public void Dispose()
		{
			this.HostCreating = null;
			this.HostCreated = null;
			this.HostCreationFailed = null;
		}
	}
}
