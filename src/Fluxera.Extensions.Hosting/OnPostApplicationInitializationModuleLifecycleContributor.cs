namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class OnPostApplicationInitializationModuleLifecycleContributor : ModuleLifecycleContributorBase
	{
		public override void Initialize(IApplicationInitializationContext context, IModule module)
		{
			(module as IOnPostApplicationInitialization)?.PostConfigure(context);
		}
	}
}