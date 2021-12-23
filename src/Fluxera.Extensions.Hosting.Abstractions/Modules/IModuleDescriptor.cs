namespace Fluxera.Extensions.Hosting.Modules
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a module descriptor that provides meta data of a module.
	/// </summary>
	[PublicAPI]
	public interface IModuleDescriptor
	{
		/// <summary>
		///     The type of the module class.
		/// </summary>
		Type Type { get; }

		/// <summary>
		///     The assembly the module is located in.
		/// </summary>
		Assembly Assembly { get; }

		/// <summary>
		///     The module instance itself.
		/// </summary>
		IModule Instance { get; }

		/// <summary>
		///     Flag, indicating if the module was loaded as a plugin.
		/// </summary>
		bool IsLoadedAsPlugin { get; }

		/// <summary>
		///     The dependent module descriptors.
		/// </summary>
		IReadOnlyCollection<IModuleDescriptor> Dependencies { get; }

		/// <summary>
		///     Gets the name of the module.
		/// </summary>
		string Name { get; }
	}
}
