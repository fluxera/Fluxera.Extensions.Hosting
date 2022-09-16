namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;
	using Microsoft.Maui.Hosting;

	/// <summary>
	///     A contract for an MAUI application host.
	/// </summary>
	[PublicAPI]
	public interface IMauiApplicationHost
	{
		/// <summary>
		///     Builds the MAUI app instance.
		/// </summary>
		/// <returns></returns>
		internal MauiApp Build();
	}
}
