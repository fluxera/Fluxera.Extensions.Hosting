namespace Fluxera.Extensions.Hosting
{
	using Fluxera.Extensions.Hosting.Modules;

	internal abstract class ModuleLifecycleContributorBase : IModuleLifecycleContributor
	{
		public virtual void Initialize(IApplicationInitializationContext context, IModule module)
		{
		}

		public virtual void Shutdown(IApplicationShutdownContext context, IModule module)
		{
		}
	}
}
