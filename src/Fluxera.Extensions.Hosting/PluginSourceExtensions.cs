namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Fluxera.Utilities.Extensions;

	internal static class PluginSourceExtensions
	{
		public static IEnumerable<Type> GetModulesWithAllDependencies(this IPluginSource pluginSource)
		{
			return pluginSource
				.GetModules()
				.SelectMany(ModuleHelper.FindDependedModuleTypesRecursiveIncludingGivenModule)
				.Distinct()
				.AsReadOnly();
		}
	}
}
