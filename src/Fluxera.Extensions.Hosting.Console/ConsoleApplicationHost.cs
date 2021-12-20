namespace Fluxera.Extensions.Hosting
{
	using System.IO;
	using System.Reflection;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Hosting;

	[PublicAPI]
	public abstract class ConsoleApplicationHost<TStartupModule> : ApplicationHost<TStartupModule>
		where TStartupModule : class, IModule
	{
		protected override void ConfigureHostBuilder(IHostBuilder builder)
		{
			// Configure the content root to use.
			builder.UseContentRoot(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

			// Configure to use the console lifetime.
			builder.UseConsoleLifetime();
		}
	}
}
