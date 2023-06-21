namespace ConsoleApp1
{
	using Autofac.Extensions.DependencyInjection;
	using Fluxera.Extensions.Hosting;
	using Fluxera.Extensions.Hosting.Plugins;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;
	using Serilog;
	using Serilog.Extensions.Hosting;
	using Serilog.Extensions.Logging;

	[PublicAPI]
	[UsedImplicitly]
	internal sealed class ConsoleApp1Host : ConsoleApplicationHost<ConsoleApp1Module> // WindowsServiceApplicationHost<ConsoleApp1Module>
	{
		/// <inheritdoc />
		protected override void ConfigureApplicationPlugins(IPluginConfigurationContext context)
		{
			base.ConfigureApplicationPlugins(context);
		}

		/// <inheritdoc />
		protected override void ConfigureHostBuilder(IHostBuilder builder)
		{
			base.ConfigureHostBuilder(builder);

			// Use Autofac as default container.
			builder.UseServiceProviderFactory(new AutofacServiceProviderFactory());

			// Use Serilog as default logger.
			builder.UseSerilog((context, services, configuration) => configuration
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.WriteTo.OpenTelemetry() // https://github.com/serilog/serilog-sinks-opentelemetry
				.ReadFrom.Configuration(context.Configuration)
				.ReadFrom.Services(services));
		}

		/// <inheritdoc />
		protected override ILoggerFactory CreateBootstrapperLoggerFactory(IConfiguration configuration)
		{
			ReloadableLogger bootstrapLogger = new LoggerConfiguration()
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.WriteTo.OpenTelemetry() // https://github.com/serilog/serilog-sinks-opentelemetry
				.ReadFrom.Configuration(configuration)
				.CreateBootstrapLogger();

			ILoggerFactory loggerFactory = new SerilogLoggerFactory(bootstrapLogger);
			return loggerFactory;
		}
	}
}
