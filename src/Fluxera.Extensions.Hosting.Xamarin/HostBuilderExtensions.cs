namespace Fluxera.Extensions.Hosting
{
	using System;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;

	/// <summary>
	///     Extensions for <see cref="IHostBuilder" />.
	/// </summary>
	[PublicAPI]
	public static class HostBuilderExtensions
	{
		/// <summary>
		///     Convenience method for adding an <see cref="IHostedService" />.
		/// </summary>
		/// <typeparam name="T">The type of service to add.</typeparam>
		/// <param name="hostBuilder">The <see cref="IHostBuilder" /> to configure.</param>
		/// <returns>The same instance of the <see cref="IHostBuilder" /> for chaining.</returns>
		public static IHostBuilder UseHostedService<T>(this IHostBuilder hostBuilder)
			where T : class, IHostedService
		{
			return hostBuilder.ConfigureServices(services => services.AddHostedService<T>());
		}

		/// <summary>
		///     Adds support for Xamarin Sleep and Resume lifecycle events.
		/// </summary>
		/// <typeparam name="TApplication">The application class.</typeparam>
		/// <param name="hostBuilder">The <see cref="IHostBuilder" /> to configure.</param>
		/// <returns>The same instance of the <see cref="IHostBuilder" /> for chaining</returns>
		public static IHostBuilder UseXamarinLifetime<TApplication>(this IHostBuilder hostBuilder)
			where TApplication : class
		{
			return hostBuilder.ConfigureServices((context, collection) =>
			{
				collection.AddSingleton<TApplication>();
				collection.AddSingleton<IHostApplicationLifetime, XamarinHostApplicationLifetime>();
				collection.AddSingleton<IHostLifetime, XamarinHostLifetime>();
			});
		}

		/// <summary>
		///     Adds support for Xamarin Sleep and Resume lifecycle events.
		/// </summary>
		/// <typeparam name="TApplication">The application class.</typeparam>
		/// <param name="hostBuilder">The <see cref="IHostBuilder" /> to configure.</param>
		/// <param name="configureOptions">The options to be configured.</param>
		/// <returns>The same instance of the <see cref="IHostBuilder" /> for chaining</returns>
		/// <summary>
		public static IHostBuilder UseXamarinLifetime<TApplication>(this IHostBuilder hostBuilder, Action<XamarinHostLifetime> configureOptions)
			where TApplication : class
		{
			return hostBuilder.ConfigureServices((context, collection) =>
			{
				UseXamarinLifetime<TApplication>(hostBuilder);
				collection.Configure(configureOptions);
			});
		}
	}
}
