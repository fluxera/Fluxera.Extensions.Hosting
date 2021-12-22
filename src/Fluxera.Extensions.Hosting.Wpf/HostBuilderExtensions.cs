namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Windows;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;

	/// <summary>
	///     Extension methods to configure the WFP host.
	/// </summary>
	[PublicAPI]
	public static class HostBuilderExtensions
	{
		/// <summary>
		///     Configures the main <see cref="Window" /> to use for the application.
		/// </summary>
		/// <typeparam name="TMainWindow">The type of the main window.</typeparam>
		/// <param name="hostBuilder">The host builder.</param>
		/// <param name="mainWindowFactory">The factory function that creates the main window instance.</param>
		/// <returns></returns>
		public static IHostBuilder UseMainWindow<TMainWindow>(this IHostBuilder hostBuilder, Func<IServiceProvider, TMainWindow> mainWindowFactory)
			where TMainWindow : Window, IMainWindow
		{
			hostBuilder.ConfigureServices(services =>
			{
				services.AddSingleton<IMainWindow>(mainWindowFactory.Invoke);
			});

			return hostBuilder;
		}

		/// <summary>
		///     Configures the host to use the <see cref="WpfApplicationLifetime" /> lifetime implementation.
		/// </summary>
		/// <param name="hostBuilder">The host builder.</param>
		/// <param name="shutdownMode">The shutdown mode the application should use.</param>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException"></exception>
		public static IHostBuilder UseWpfApplicationLifetime(this IHostBuilder hostBuilder, ShutdownMode shutdownMode)
		{
			// Configure the WPF lifetime.
			hostBuilder.ConfigureServices((context, services) =>
			{
				if(!context.Properties.TryRetrieveContext(WpfContext.ContextKey, out WpfContext wpfContext))
				{
					throw new InvalidOperationException("The WPF defaults need to be configured.");
				}

				services.AddSingleton<IHostLifetime, WpfApplicationLifetime>();

				wpfContext.ShutdownMode = shutdownMode;
			});

			return hostBuilder;
		}
	}
}
