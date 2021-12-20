namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class OnApplicationShutdownModuleLifecycleContributor : ModuleLifecycleContributorBase
	{
		public override void Shutdown(IApplicationShutdownContext context, IModule module)
		{
			(module as IOnApplicationShutdown)?.OnApplicationShutdown(context);
		}
	}
}