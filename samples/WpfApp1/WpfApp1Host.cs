namespace WpfApp1
{
	using Fluxera.Extensions.Hosting;
	using Microsoft.Extensions.Hosting;

	public class WpfApp1Host : WpfApplicationHost<WpfApp1Module>
	{
		/// <inheritdoc />
		protected override void ConfigureHostBuilder(IHostBuilder builder)
		{
			base.ConfigureHostBuilder(builder);

			// Register the main window factory to use.
			builder.UseMainWindow(serviceProvider => new MainWindow());
		}

		///// <inheritdoc />
		//protected override void ConfigureApplicationHostEvents(ApplicationHostEvents applicationHostEvents)
		//{
		//	bool showSplash = !this.CommandLineArgs.Contains("--no-splash");
		//	if(showSplash)
		//	{
		//		applicationHostEvents.HostCreating
		//	}
		//}
	}
}
