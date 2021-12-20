namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Collections.Generic;
	using Microsoft.Extensions.DependencyInjection;

	public delegate IApplicationLoader ApplicationLoaderBuilderFunc(
		Type startupModuleType,
		IServiceCollection services,
		IPluginSourceList pluginSources,
		IReadOnlyCollection<IModuleDescriptor> modules);
}
