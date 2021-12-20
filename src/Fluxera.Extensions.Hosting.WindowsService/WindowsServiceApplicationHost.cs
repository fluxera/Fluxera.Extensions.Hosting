namespace Fluxera.Extensions.Hosting
{
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Hosting;

	[PublicAPI]
	public abstract class WindowsServiceApplicationHost<TStartupModule> : ApplicationHost<TStartupModule>
		where TStartupModule : class, IModule
	{
		protected override void ConfigureHostBuilder(IHostBuilder builder)
		{
			bool isService = !(Debugger.IsAttached || this.CommandLineArgs.Contains("--console"));

			// Configure the content root to use.
			builder.UseContentRoot(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

			if(isService)
			{
				// Configure to use the windows service lifetime.
				builder.UseWindowsService();
			}
			else
			{
				// Configure to use the console lifetime.
				builder.UseConsoleLifetime();
			}
		}
	}
}
