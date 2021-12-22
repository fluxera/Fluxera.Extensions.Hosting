namespace Fluxera.Extensions.Hosting
{
	using System.Collections.Generic;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a container holding the <see cref="IModuleDescriptor" />
	///     instances of loader modules.
	/// </summary>
	[PublicAPI]
	public interface IModuleContainer
	{
		/// <summary>
		///     Gets the descriptors of the loaded modules.
		/// </summary>
		IReadOnlyCollection<IModuleDescriptor> Modules { get; }
	}
}
