namespace Fluxera.Extensions.Hosting.Plugins
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class PluginSourceList : List<IPluginSource>, IPluginSourceList
	{
		IEnumerable<Assembly> IPluginSourceList.GetAllAssemblies()
		{
			return this
				.SelectMany(pluginSource => pluginSource.GetAssemblies())
				.Distinct();
		}

		IEnumerable<Type> IPluginSourceList.GetAllModules()
		{
			return this
				.SelectMany(pluginSource => pluginSource.GetModulesWithAllDependencies())
				.Distinct();
		}
	}
}
