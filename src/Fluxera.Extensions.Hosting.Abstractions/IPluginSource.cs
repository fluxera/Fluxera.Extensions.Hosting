namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IPluginSource
	{
		IEnumerable<Assembly> GetAssemblies();

		IEnumerable<Type> GetModules();
	}
}
