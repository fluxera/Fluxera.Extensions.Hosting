namespace ConsoleApp1
{
	using Fluxera.Extensions.Hosting;
	using Fluxera.Extensions.Hosting.Plugins;
	using JetBrains.Annotations;

	[PublicAPI]
	[UsedImplicitly]
	internal sealed class ConsoleApp1Host : ConsoleApplicationHost<ConsoleApp1Module> // WindowsServiceApplicationHost<ConsoleApp1Module>
	{
		/// <inheritdoc />
		protected override void ConfigureApplicationPlugins(IPluginConfigurationContext context)
		{
			base.ConfigureApplicationPlugins(context);
		}

		///// <inheritdoc />
		//protected override void ConfigureHostBuilder(IHostBuilder builder)
		//{
		//	base.ConfigureHostBuilder(builder);

		//	// Use Autofac as default container.
		//	builder.UseServiceProviderFactory(new AutofacServiceProviderFactory());

		//	// Use Serilog as default logger.
		//	builder.UseSerilog((context, services, configuration) => configuration
		//		.Enrich.FromLogContext()
		//		.WriteTo.Console()
		//		.ReadFrom.Configuration(context.Configuration)
		//		.ReadFrom.Services(services));
		//}

		///// <inheritdoc />
		//protected override ILoggerFactory CreateBootstrapperLoggerFactory(IConfiguration configuration)
		//{
		//	ReloadableLogger? bootstrapLogger = new LoggerConfiguration()
		//		.Enrich.FromLogContext()
		//		.WriteTo.Console()
		//		.ReadFrom.Configuration(configuration)
		//		.CreateBootstrapLogger();

		//	ILoggerFactory loggerFactory = new SerilogLoggerFactory(bootstrapLogger);
		//	return loggerFactory;
		//}
	}
}
