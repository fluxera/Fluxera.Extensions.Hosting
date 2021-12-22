namespace Fluxera.Extensions.Hosting
{
	using System.Windows;
	using System.Windows.Threading;

	internal sealed class WpfContext
	{
		internal const string ContextKey = "WpfContext";

		public Dispatcher Dispatcher => this.Application.Dispatcher;

		public ShutdownMode ShutdownMode { get; set; } = ShutdownMode.OnLastWindowClose;

		public Application Application { get; set; } = null!;
	}
}
