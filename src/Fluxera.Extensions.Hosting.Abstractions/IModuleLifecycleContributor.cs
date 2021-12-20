namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IModuleLifecycleContributor
	{
		void Initialize(IApplicationInitializationContext context, IModule module);

		void Shutdown(IApplicationShutdownContext context, IModule module);
	}
}
