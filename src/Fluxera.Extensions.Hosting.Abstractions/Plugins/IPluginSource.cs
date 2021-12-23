namespace Fluxera.Extensions.Hosting.Plugins
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using Fluxera.Extensions.Hosting.Modules;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a plugin source.
	/// </summary>
	[PublicAPI]
	public interface IPluginSource
	{
		/// <summary>
		///     Gets the assemblies of the <see cref="IModule" /> types.
		/// </summary>
		/// <returns></returns>
		IEnumerable<Assembly> GetAssemblies();

		/// <summary>
		///     Gets the <see cref="IModule" /> types.
		/// </summary>
		/// <returns></returns>
		IEnumerable<Type> GetModules();
	}
}
