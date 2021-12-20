namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IModuleDescriptor
	{
		Type Type { get; }

		Assembly Assembly { get; }

		IModule Instance { get; }

		bool IsLoadedAsPlugin { get; }

		IReadOnlyCollection<IModuleDescriptor> Dependencies { get; }

		string Name { get; }
	}
}
