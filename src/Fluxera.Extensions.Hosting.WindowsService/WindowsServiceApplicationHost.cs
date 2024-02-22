namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Diagnostics;
	using System.Linq;
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
			builder.UseContentRoot(Environment.CurrentDirectory);

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
