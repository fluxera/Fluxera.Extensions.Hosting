namespace Fluxera.Extensions.Hosting
{
	using System;
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IModuleDependencyConfigurator : IDependentTypesProvider
	{
		IModuleDependencyConfigurator On<TModule>() where TModule : class, IModule;

		IModuleDependencyConfigurator On(Type moduleType);
	}
}
