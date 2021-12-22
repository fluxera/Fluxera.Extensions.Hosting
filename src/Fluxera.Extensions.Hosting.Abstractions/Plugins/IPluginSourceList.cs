namespace Fluxera.Extensions.Hosting.Plugins
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using Fluxera.Extensions.Hosting.Modules;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for providing plugin module types.
	/// </summary>
	[PublicAPI]
	public interface IPluginSourceList : ICollection<IPluginSource>
	{
		/// <summary>
		///     Gets the assemblies that contain <see cref="IModule" /> types from
		///     all available <see cref="IPluginSource" /> instances.
		/// </summary>
		/// <returns></returns>
		IEnumerable<Assembly> GetAllAssemblies();

		/// <summary>
		///     Gets all <see cref="IModule" /> types from all available
		///     <see cref="IPluginSource" /> instances including their dependencies.
		/// </summary>
		/// <returns></returns>
		IEnumerable<Type> GetAllModules();
	}
}
