namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Collections.Generic;
	using Fluxera.Extensions.Hosting.Modules;
	using Fluxera.Extensions.Hosting.Plugins;
	using Microsoft.Extensions.DependencyInjection;

	public delegate IApplicationLoader ApplicationLoaderBuilderFunc(
		Type startupModuleType,
		IServiceCollection services,
		IPluginSourceList pluginSources,
		IReadOnlyCollection<IModuleDescriptor> modules);
}
