namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Collections.Generic;
	using Fluxera.Extensions.Hosting.Plugins;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;

	[PublicAPI]
	public interface IModuleLoader
	{
		/// <summary>
		/// </summary>
		/// <param name="startupModuleType"></param>
		/// <param name="services"></param>
		/// <param name="pluginSources"></param>
		/// <returns></returns>
		IReadOnlyCollection<IModuleDescriptor> LoadModules(
			Type startupModuleType,
			IServiceCollection services,
			IPluginSourceList pluginSources);
	}
}
