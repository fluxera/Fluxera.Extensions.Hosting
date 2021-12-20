namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class OnPreApplicationInitializationModuleLifecycleContributor : ModuleLifecycleContributorBase
	{
		public override void Initialize(IApplicationInitializationContext context, IModule module)
		{
			(module as IOnPreApplicationInitialization)?.PreConfigure(context);
		}
	}
}