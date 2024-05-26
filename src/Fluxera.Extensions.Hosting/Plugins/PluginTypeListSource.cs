namespace Fluxera.Extensions.Hosting.Plugins
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	/// <summary>
	///     A plugin source for manually adding module types.
	/// </summary>
	internal sealed class PluginTypeListSource : IPluginSource
	{
		private readonly Lazy<IEnumerable<Assembly>> moduleAssemblies;
		private readonly Type[] moduleTypes;

		public PluginTypeListSource(params Type[] moduleTypes)
		{
			Guard.ThrowIfNull(moduleTypes);

			this.moduleTypes = moduleTypes;

			this.moduleAssemblies = new Lazy<IEnumerable<Assembly>>(this.LoadAssemblies, true);
		}

		public IEnumerable<Assembly> GetAssemblies()
		{
			return this.moduleAssemblies.Value;
		}

		public IEnumerable<Type> GetModules()
		{
			return this.moduleTypes;
		}

		private IEnumerable<Assembly> LoadAssemblies()
		{
			return this.moduleTypes.Select(type => type.GetTypeInfo().Assembly);
		}
	}
}
