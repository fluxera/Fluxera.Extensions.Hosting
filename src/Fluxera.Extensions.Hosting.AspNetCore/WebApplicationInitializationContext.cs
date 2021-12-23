namespace Fluxera.Extensions.Hosting
{
	using System;
	using Fluxera.Guards;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;

	internal sealed class WebApplicationInitializationContext : IApplicationInitializationContext
	{
		public WebApplicationInitializationContext(IApplicationBuilder applicationBuilder)
		{
			Guard.Against.Null(applicationBuilder, nameof(applicationBuilder));

			this.ApplicationBuilder = applicationBuilder;
			this.ServiceProvider = applicationBuilder.ApplicationServices;

			this.Configuration = this.ServiceProvider.GetRequiredService<IConfiguration>();
			this.Environment = this.ServiceProvider.GetRequiredService<IHostEnvironment>();
			ILoggerFactory loggerFactory = this.ServiceProvider.GetRequiredService<ILoggerFactory>();
			this.Logger = loggerFactory.CreateLogger(ApplicationHost.LoggerName);
		}

		public IApplicationBuilder ApplicationBuilder { get; }

		/// <inheritdoc />
		public IServiceProvider ServiceProvider { get; }

		/// <inheritdoc />
		public IConfiguration Configuration { get; }

		/// <inheritdoc />
		public IHostEnvironment Environment { get; }

		/// <inheritdoc />
		public ILogger Logger { get; set; }
	}
}
