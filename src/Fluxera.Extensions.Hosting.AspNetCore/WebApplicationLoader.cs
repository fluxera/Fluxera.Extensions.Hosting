namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Collections.Generic;
	using Microsoft.Extensions.DependencyInjection;

	internal sealed class WebApplicationLoader : ApplicationLoader
	{
		private WebApplicationLoaderInitializationContext webContext;

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

			this.webContext = this.webContext;
		}

		protected override IApplicationInitializationContext CreateApplicationInitializationContext(IServiceProvider serviceProvider)
		{
			return new WebApplicationInitializationContext(this.webContext.ApplicationBuilder);
		}
	}
}
