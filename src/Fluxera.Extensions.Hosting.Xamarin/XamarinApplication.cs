namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;
	using Microsoft.Extensions.Hosting;
	using Xamarin.Forms;

	/// <summary>
	///     An abstract base class for Xamarin Forms application classes.
	/// </summary>
	[PublicAPI]
	public abstract class XamarinApplication : Application
	{
		/// <summary>
		///     Gets or sets the host.
		/// </summary>
		public IHost Host { get; internal set; } = null!;
	}
}
