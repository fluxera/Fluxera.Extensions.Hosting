﻿namespace Fluxera.Extensions.Hosting
{
	using System;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;

	internal class ApplicationInitializationContext : IApplicationInitializationContext
	{
		public ApplicationInitializationContext(IServiceProvider serviceProvider)
		{
			Guard.ThrowIfNull(serviceProvider);

			this.ServiceProvider = serviceProvider;
			this.Configuration = this.ServiceProvider.GetRequiredService<IConfiguration>();
			this.Environment = this.ServiceProvider.GetRequiredService<IHostEnvironment>();
			ILoggerFactory loggerFactory = this.ServiceProvider.GetRequiredService<ILoggerFactory>();
			this.Logger = loggerFactory.CreateLogger(ApplicationHost.LoggerName);
		}

		/// <inheritdoc />
		public IServiceProvider ServiceProvider { get; }

		/// <inheritdoc />
		public IConfiguration Configuration { get; }

		/// <inheritdoc />
		public IHostEnvironment Environment { get; }

		/// <inheritdoc />
		public ILogger Logger { get; }

		/// <inheritdoc />
		IServiceProvider ILoggingContext<IServiceProvider>.LogContextData => this.ServiceProvider;
	}
}
