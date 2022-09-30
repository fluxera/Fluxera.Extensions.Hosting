namespace Fluxera.Extensions.Hosting
{
	using System;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.FileProviders;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;
	using Xamarin.Forms;

	/// <summary>
	///     Provides convenience methods for creating instances of <see cref="IHost" /> and <see cref="IHostBuilder" /> with
	///     pre-configured defaults.
	/// </summary>
	[PublicAPI]
	[Obsolete("The hosting library for Xamarin.Forms will be remove in the 7.0 release.")]
	public static class XamarinHost
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="IHostBuilder" /> class with pre-configured defaults.
		/// </summary>
		/// <typeparam name="TApplication">The type containing the static <see cref="IHost" /> method.</typeparam>
		/// <returns>The initialized <see cref="IHostBuilder" />.</returns>
		public static IHostBuilder CreateDefaultBuilder<TApplication>()
			where TApplication : Application
		{
			IHostBuilder builder = new HostBuilder();

			builder.ConfigureAppConfiguration((hostingContext, configurationBuilder) =>
			{
				IHostEnvironment environment = hostingContext.HostingEnvironment;

				configurationBuilder.SetFileProvider(new EmbeddedFileProvider(typeof(TApplication).Assembly));
				configurationBuilder.AddJsonFile("appsettings.json", true);
				configurationBuilder.AddJsonFile($"appsettings.{environment.EnvironmentName}.json", true);
			});

			builder.ConfigureLogging((hostingContext, logging) =>
			{
				logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
				logging.AddDebug();
			});

			builder.UseDefaultServiceProvider((context, options) =>
			{
				options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
			});

			builder.UseXamarinLifetime<TApplication>();

			if(Device.RuntimePlatform == Device.Android)
			{
				// Set the content root for android.
				builder.UseContentRoot(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
			}

			return builder;
		}
	}
}
