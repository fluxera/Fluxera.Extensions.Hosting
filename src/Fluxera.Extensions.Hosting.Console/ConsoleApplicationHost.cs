namespace Fluxera.Extensions.Hosting
{
	using System.IO;
	using System.Reflection;
	using Fluxera.Extensions.Hosting.Modules;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Hosting;

	/// <summary>
	///     An abstract base class for console application hosts.
	/// </summary>
	/// <typeparam name="TStartupModule">The startup module type.</typeparam>
	[PublicAPI]
	public abstract class ConsoleApplicationHost<TStartupModule> : ApplicationHost<TStartupModule>
		where TStartupModule : class, IModule
	{
		/// <inheritdoc />
		protected override void ConfigureHostBuilder(IHostBuilder builder)
		{
			// Configure the content root to use.
			builder.UseContentRoot(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

			// Configure to use the console lifetime.
			builder.UseConsoleLifetime();
		}
	}
}
