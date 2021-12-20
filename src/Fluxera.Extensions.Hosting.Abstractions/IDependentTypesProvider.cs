namespace Fluxera.Extensions.Hosting
{
	using System;
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IDependentTypesProvider
	{
		Type[] GetDependentTypes();
	}
}
