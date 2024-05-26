namespace Fluxera.Extensions.Hosting.Plugins
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	internal static class PluginSourceExtensions
	{
		public static IEnumerable<Type> GetModulesWithAllDependencies(this IPluginSource pluginSource)
		{
			return pluginSource
				.GetModules()
				.SelectMany(ModuleHelper.FindDependedModuleTypesRecursiveIncludingGivenModule)
				.Distinct();
		}
	}
}
