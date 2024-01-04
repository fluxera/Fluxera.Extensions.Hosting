namespace BlazorApp2.Client
{
	using Fluxera.Extensions.Hosting;
	using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
	using Microsoft.Extensions.Logging;
	using Serilog;
	using Serilog.Core;
	using Serilog.Extensions.Logging;

	internal sealed class BlazorApp2Host : BlazorApplicationHost<BlazorApp2Module>
	{
		/// <inheritdoc />
		protected override void ConfigureHostBuilder(WebAssemblyHostBuilder builder)
		{
			base.ConfigureHostBuilder(builder);

			// Use Serilog as default logger.
			// https://github.com/serilog/serilog-sinks-browserconsole
			Logger logger = new LoggerConfiguration()
				.MinimumLevel.Verbose()
				.WriteTo.BrowserConsole()
				.CreateLogger();

			builder.Logging.AddSerilog(logger);
		}

		/// <inheritdoc />
		protected override ILoggerFactory CreateBootstrapperLoggerFactory()
		{
			// https://github.com/serilog/serilog-sinks-browserconsole
			Logger bootstrapLogger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.WriteTo.BrowserConsole()
				.CreateLogger();

			ILoggerFactory loggerFactory = new SerilogLoggerFactory(bootstrapLogger);
			return loggerFactory;
		}
	}
}
