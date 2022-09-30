namespace Fluxera.Extensions.Hosting
{
	using System;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Hosting;
	using Xamarin.Forms;

	/// <summary>
	///     An abstract base class for Xamarin Forms application classes.
	/// </summary>
	[PublicAPI]
	[Obsolete("The hosting library for Xamarin.Forms will be remove in the 7.0 release.")]
	public abstract class XamarinApplication : Application
	{
		/// <summary>
		///     Gets or sets the host.
		/// </summary>
		public IHost Host { get; internal set; }
	}
}
