﻿namespace Fluxera.Extensions.Hosting
{
	/// <summary>
	/// Options for configuring the hosting environment.
	/// </summary>
	public class XamarinHostLifetimeOptions
	{
		/// <summary>
		/// Indicates if host lifetime status messages should be suppressed such as on startup.
		/// The default is false.
		/// </summary>
		public bool SuppressStatusMessages { get; set; }
	}
}