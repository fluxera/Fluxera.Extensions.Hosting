namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Windows;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;

	[PublicAPI]
	public static class HostBuilderExtensions
	{
		public static IHostBuilder UseMainWindow<TMainWindow>(this IHostBuilder hostBuilder, Func<TMainWindow> mainWindowFactory)
			where TMainWindow : Window, IMainWindow
		{
			hostBuilder.ConfigureServices((context, services) =>
			{
				services.AddSingleton<IMainWindow>(serviceProvider => mainWindowFactory.Invoke());
			});

			return hostBuilder;
		}

		internal static IHostBuilder UseWpfApplicationLifetime(this IHostBuilder hostBuilder, ShutdownMode shutdownMode)
		{
			// Configure the WPF lifetime.
			hostBuilder.ConfigureServices((context, services) =>
			{
				if(!context.Properties.TryRetrieveContext(WpfContext.ContextKey, out WpfContext wpfContext))
				{
					throw new InvalidOperationException("The presentation defaults need to ne configured first.");
				}

				services.AddSingleton<IHostLifetime, WpfApplicationLifetime>();

				wpfContext.ShutdownMode = shutdownMode;
			});

			return hostBuilder;
		}
	}
}
