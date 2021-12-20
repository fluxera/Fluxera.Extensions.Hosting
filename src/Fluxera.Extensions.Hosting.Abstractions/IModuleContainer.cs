namespace Fluxera.Extensions.Hosting
{
	using System.Collections.Generic;
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IModuleContainer
	{
		IReadOnlyCollection<IModuleDescriptor> Modules { get; }
	}
}
