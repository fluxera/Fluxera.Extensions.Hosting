namespace Fluxera.Extensions.Hosting
{
	using System;

	/// <summary>
	///     Options for configuring the hosting environment.
	/// </summary>
	[Obsolete("The hosting library for Xamarin.Forms will be remove in the 7.0 release.")]
	public sealed class XamarinHostLifetimeOptions
	{
		/// <summary>
		///     Indicates if host lifetime status messages should be suppressed such as on startup.
		///     The default is false.
		/// </summary>
		public bool SuppressStatusMessages { get; set; }
	}
}
