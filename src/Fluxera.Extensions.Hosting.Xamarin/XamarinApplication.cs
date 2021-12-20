namespace Fluxera.Extensions.Hosting
{
	using global::Xamarin.Forms;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Hosting;

	[PublicAPI]
	public abstract class XamarinApplication : Application
	{
		public IHost Host { get; internal set; }
	}
}
