namespace Fluxera.Extensions.Hosting
{
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using Fluxera.Extensions.Hosting.Modules;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Hosting;

	/// <summary>
	///     An abstract base class for windows service application hosts.
	/// </summary>
	/// <typeparam name="TStartupModule">The startup module type.</typeparam>
	[PublicAPI]
	public abstract class WindowsServiceApplicationHost<TStartupModule> : ApplicationHost<TStartupModule>
		where TStartupModule : class, IModule
	{
		/// <inheritdoc />
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
