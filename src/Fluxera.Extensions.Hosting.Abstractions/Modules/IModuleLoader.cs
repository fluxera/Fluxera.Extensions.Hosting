namespace Fluxera.Extensions.Hosting.Modules
{
	using System;
	using System.Collections.Generic;
	using Fluxera.Extensions.Hosting.Plugins;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;

	/// <summary>
	///     A contract for a service that loads modules.
	/// </summary>
	[PublicAPI]
	public interface IModuleLoader
	{
		/// <summary>
		///     Loads all modules starting with the given startup module and
		///     then loading it's dependencies.
		/// </summary>
		/// <param name="startupModuleType"></param>
		/// <param name="services"></param>
		/// <param name="pluginSources"></param>
		/// <returns></returns>
		IReadOnlyCollection<IModuleDescriptor> LoadModules(Type startupModuleType, IServiceCollection services, IPluginSourceList pluginSources);
	}
}
