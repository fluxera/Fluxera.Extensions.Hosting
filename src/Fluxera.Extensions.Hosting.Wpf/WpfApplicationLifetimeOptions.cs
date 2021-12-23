namespace Fluxera.Extensions.Hosting
{
	/// <summary>
	///     Provides lifetime options for the WPF application.
	/// </summary>
	public sealed class WpfApplicationLifetimeOptions
	{
		/// <summary>
		///     Creates a new instance of the <see cref="WpfApplicationLifetimeOptions" /> type.
		/// </summary>
		public WpfApplicationLifetimeOptions()
		{
			this.SuppressStatusMessages = false;
		}

		/// <summary>
		///     Flag, if the status messages should be suppressed.
		/// </summary>
		public bool SuppressStatusMessages { get; }
	}
}
