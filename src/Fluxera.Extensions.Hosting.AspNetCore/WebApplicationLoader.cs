namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Collections.Generic;
	using Fluxera.Extensions.Hosting.Modules;
	using Fluxera.Extensions.Hosting.Plugins;
	using Microsoft.Extensions.DependencyInjection;

	/// <summary>
	///     A specialized <see cref="IApplicationLoader" /> for web applications.
	/// </summary>
	internal sealed class WebApplicationLoader : ApplicationLoader
	{
		private WebApplicationLoaderInitializationContext webContext = null!;

		public WebApplicationLoader(
			Type startupModuleType,
			IServiceCollection services,
			IPluginSourceList pluginSources,
			IReadOnlyCollection<IModuleDescriptor> modules)
			: base(startupModuleType, services, pluginSources, modules)
		{
		}

		/// <inheritdoc />
		public override void Initialize(IApplicationLoaderInitializationContext context)
		{
			base.Initialize(context);

			this.webContext = (WebApplicationLoaderInitializationContext)context;
		}

		protected override IApplicationInitializationContext CreateApplicationInitializationContext(IServiceProvider serviceProvider)
		{
			return new WebApplicationInitializationContext(this.webContext.ApplicationBuilder);
		}
	}
}
