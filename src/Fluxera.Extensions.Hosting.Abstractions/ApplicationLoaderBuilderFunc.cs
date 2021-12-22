namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Collections.Generic;
	using Fluxera.Extensions.Hosting.Modules;
	using Fluxera.Extensions.Hosting.Plugins;
	using Microsoft.Extensions.DependencyInjection;

	/// <summary>
	///     A function that builds and returns the <see cref="IApplicationLoader" />
	///     from the given parameters.
	/// </summary>
	/// <param name="startupModuleType">The startup module type.</param>
	/// <param name="services">The service collection.</param>
	/// <param name="pluginSources">The optional plugin sources.</param>
	/// <param name="modules">The modules.</param>
	/// <returns></returns>
	public delegate IApplicationLoader ApplicationLoaderBuilderFunc(
		Type startupModuleType,
		IServiceCollection services,
		IPluginSourceList pluginSources,
		IReadOnlyCollection<IModuleDescriptor> modules);
}
