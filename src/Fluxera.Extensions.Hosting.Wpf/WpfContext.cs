namespace Fluxera.Extensions.Hosting
{
	using System.Windows;
	using System.Windows.Threading;

	internal sealed class WpfContext : IWpfContext
	{
		internal const string ContextKey = "WpfContext";

		/// <inheritdoc />
		public Dispatcher Dispatcher => this.Application.Dispatcher;

		/// <inheritdoc />
		public ShutdownMode ShutdownMode { get; set; } = ShutdownMode.OnLastWindowClose;

		/// <inheritdoc />
		public Application Application { get; set; }
	}
}
