namespace Fluxera.Extensions.Hosting
{
	using System;

	/// <summary>
	///     A class that provides several events that provide feedback of the host creation cycle.
	/// </summary>
	public sealed class ApplicationHostEvents : IDisposable
	{
		/// <inheritdoc />
		public void Dispose()
		{
			this.HostCreating = null;
			this.HostCreated = null;
			this.HostCreationFailed = null;
		}

		/// <summary>
		///     An event that is raised before the host is created.
		/// </summary>
		public event EventHandler HostCreating;

		/// <summary>
		///     An event that is raised after the host was created.
		/// </summary>
		public event EventHandler HostCreated;

		/// <summary>
		///     An event that is raised, if the host creation failed.
		/// </summary>
		public event EventHandler<HostInitializationFailedEventArgs> HostCreationFailed;

		/// <summary>
		///     Raise the <see cref="HostCreating" /> event.
		/// </summary>
		public void OnHostCreating()
		{
			this.HostCreating?.Invoke(null, EventArgs.Empty);
		}

		/// <summary>
		///     Raise the <see cref="HostCreated" /> event.
		/// </summary>
		public void OnHostCreated()
		{
			this.HostCreated?.Invoke(null, EventArgs.Empty);
		}

		/// <summary>
		///     Raise the <see cref="HostCreationFailed" /> event.
		/// </summary>
		/// <param name="exception"></param>
		public void OnHostCreationFailed(Exception exception)
		{
			this.HostCreationFailed?.Invoke(null, new HostInitializationFailedEventArgs(exception));
		}
	}
}
