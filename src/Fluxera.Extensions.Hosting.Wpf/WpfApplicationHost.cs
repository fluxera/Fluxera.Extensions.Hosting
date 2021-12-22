namespace Fluxera.Extensions.Hosting
{
	using System.Windows;
	using Fluxera.Extensions.Hosting.Modules;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;

	[PublicAPI]
	public abstract class WpfApplicationHost<TStartupModule> : ApplicationHost<TStartupModule>
		where TStartupModule : class, IModule
	{
		/// <inheritdoc />
		protected override void ConfigureHostBuilder(IHostBuilder builder)
		{
			base.ConfigureHostBuilder(builder);

			// Configure the WPF context, hosted service and UI thread.
			builder.ConfigureServices((context, services) =>
			{
				// Only execute if no context exists in the properties.
				// The context is implicitly created and added to the properties.
				if(!context.Properties.TryRetrieveContext(WpfContext.ContextKey, out WpfContext wpfContext))
				{
					services.AddSingleton<IWpfContext>(wpfContext);
				}
			});

			// Configure the WPF lifetime.
			builder.UseWpfApplicationLifetime(ShutdownMode.OnMainWindowClose);
		}
	}
}
