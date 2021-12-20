namespace Fluxera.Extensions.Hosting
{
	using System.Windows;
	using System.Windows.Threading;

	/// <summary>
	///     The WPF context contains all information about the WPF application and how it's started and stopped.
	/// </summary>
	internal interface IWpfContext
	{
		/// <summary>
		///     This WPF dispatcher.
		/// </summary>
		Dispatcher Dispatcher { get; }

		/// <summary>
		///     This is the WPF ShutdownMode used for the WPF application lifetime, default is OnLastWindowClose.
		/// </summary>
		ShutdownMode ShutdownMode { get; }

		/// <summary>
		///     The Application.
		/// </summary>
		Application Application { get; set; }
	}
}
