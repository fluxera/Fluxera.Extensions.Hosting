namespace Fluxera.Extensions.Hosting.Plugins
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;

	/// <summary>
	///     A plugin source for manually adding a module type.
	/// </summary>
	internal sealed class PluginModuleTypeSource : IPluginSource
	{
		private readonly Lazy<Assembly> moduleAssembly;
		private readonly Type moduleType;

		public PluginModuleTypeSource(Type moduleType)
		{
			Guard.ThrowIfNull(moduleType);

			this.moduleType = moduleType;
			this.moduleAssembly = new Lazy<Assembly>(this.LoadAssembly, true);
		}

		public IEnumerable<Assembly> GetAssemblies()
		{
			yield return this.moduleAssembly.Value;
		}

		public IEnumerable<Type> GetModules()
		{
			yield return this.moduleType;
		}

		private Assembly LoadAssembly()
		{
			return this.moduleType.GetTypeInfo().Assembly;
		}
	}
}
