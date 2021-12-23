namespace Fluxera.Extensions.Hosting.Modules
{
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class OnApplicationInitializationModuleLifecycleContributor : ModuleLifecycleContributorBase
	{
		public override void Initialize(IApplicationInitializationContext context, IModule module)
		{
			(module as IConfigureApplication)?.Configure(context);
		}
	}
}
