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
		public IEnumerable<Assembly> GetAllAssemblies()
		{
			return this
				.SelectMany(pluginSource => pluginSource.GetAssemblies())
				.Distinct();
		}

		public IEnumerable<Type> GetAllModules()
		{
			return this
				.SelectMany(pluginSource => pluginSource.GetModulesWithAllDependencies())
				.Distinct();
		}
	}
}
