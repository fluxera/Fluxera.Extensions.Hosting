namespace Fluxera.Extensions.Hosting
{
	using System;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;

	/// <summary>
	///     Extensions for the <see cref="IHostBuilder" /> type.
	/// </summary>
	[PublicAPI]
	public static class HostBuilderExtensions
	{
		/// <summary>
		///     Adds an <see cref="IHostedService" /> to the service collection when configuring the host builder.
		/// </summary>
		/// <typeparam name="T">The type of service to add.</typeparam>
		/// <param name="hostBuilder">The host builder.</param>
		/// <returns>T</returns>
		public static IHostBuilder UseHostedService<T>(this IHostBuilder hostBuilder)
			where T : class, IHostedService
		{
			return hostBuilder.ConfigureServices(services => services.AddHostedService<T>());
		}

		/// <summary>
		///     Adds support for the Sleep and Resume lifecycle events.
		/// </summary>
		/// <typeparam name="TApplication">The application type.</typeparam>
		/// <param name="hostBuilder">The host builder.</param>
		/// <returns></returns>
		public static IHostBuilder UseXamarinLifetime<TApplication>(this IHostBuilder hostBuilder)
			where TApplication : class
		{
			return hostBuilder.ConfigureServices(services =>
			{
				services.AddSingleton<TApplication>();
				services.AddSingleton<IHostApplicationLifetime, XamarinHostApplicationLifetime>();
				services.AddSingleton(serviceProvider => (IXamarinHostApplicationLifetime)serviceProvider.GetRequiredService<IHostApplicationLifetime>());
				services.AddSingleton<IHostLifetime, XamarinHostLifetime>();
			});
		}

		/// <summary>
		///     Adds support for Sleep and Resume lifecycle events.
		/// </summary>
		/// <typeparam name="TApplication">The application type.</typeparam>
		/// <param name="hostBuilder">The host builder.</param>
		/// <param name="configureOptions">Action to configure the options.</param>
		/// <returns></returns>
		public static IHostBuilder UseXamarinLifetime<TApplication>(this IHostBuilder hostBuilder, Action<XamarinHostLifetimeOptions> configureOptions)
			where TApplication : class
		{
			return hostBuilder.ConfigureServices(services =>
			{
				hostBuilder.UseXamarinLifetime<TApplication>();
				services.Configure(configureOptions);
			});
		}
	}
}
