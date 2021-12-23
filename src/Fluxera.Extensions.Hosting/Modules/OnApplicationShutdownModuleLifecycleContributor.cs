namespace Fluxera.Extensions.Hosting.Modules
{
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class OnApplicationShutdownModuleLifecycleContributor : ModuleLifecycleContributorBase
	{
		public override void Shutdown(IApplicationShutdownContext context, IModule module)
		{
			(module as IShutdownApplicationModule)?.OnApplicationShutdown(context);
		}
	}
}
