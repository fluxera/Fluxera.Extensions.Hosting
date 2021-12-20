namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;

	public interface IPluginSourceList : ICollection<IPluginSource>
	{
		IEnumerable<Assembly> GetAllAssemblies();

		IEnumerable<Type> GetAllModules();
	}
}
