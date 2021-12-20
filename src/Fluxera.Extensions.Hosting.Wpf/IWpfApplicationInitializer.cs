namespace Fluxera.Extensions.Hosting
{
	using System.Windows;
	using JetBrains.Annotations;

	/// <summary>
	///     This defines a service which is called before the message loop is started.
	/// </summary>
	[PublicAPI]
	public interface IWpfApplicationInitializer
	{
		/// <summary>
		///     Do whatever you need to do to initialize WPF, this is called from the UI thread.
		/// </summary>
		/// <param name="application">Application</param>
		void Initialize(Application application);
	}
}
