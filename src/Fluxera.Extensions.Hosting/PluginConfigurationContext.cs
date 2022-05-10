namespace Fluxera.Extensions.Hosting
{
	using Fluxera.Extensions.DependencyInjection;
	using Fluxera.Extensions.Hosting.Plugins;
	using Fluxera.Guards;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;

	[PublicAPI]
	internal sealed class PluginConfigurationContext : IPluginConfigurationContext
	{
		public PluginConfigurationContext(IServiceCollection services, IPluginSourceList pluginSources)
		{
			Guard.Against.Null(services, nameof(services));
			Guard.Against.Null(pluginSources, nameof(pluginSources));

			this.PluginSources = pluginSources;

			this.Configuration = services.GetObject<IConfiguration>();
			this.Environment = services.GetObject<IHostEnvironment>();
			this.Logger = services.GetObject<ILogger>();
		}

		public IPluginSourceList PluginSources { get; }

		/// <inheritdoc />
		public IConfiguration Configuration { get; }

		/// <inheritdoc />
		public IHostEnvironment Environment { get; }

		/// <inheritdoc />
		public ILogger Logger { get; }

		/// <inheritdoc />
		IPluginSourceList ILoggingContext<IPluginSourceList>.LogContextData => this.PluginSources;
	}
}
