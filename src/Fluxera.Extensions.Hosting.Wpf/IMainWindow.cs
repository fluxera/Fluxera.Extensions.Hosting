namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for the main window of a WPF application.
	/// </summary>
	[PublicAPI]
	public interface IMainWindow
	{
		/// <summary>
		///     Shows the main window.
		/// </summary>
		void Show();
	}
}
