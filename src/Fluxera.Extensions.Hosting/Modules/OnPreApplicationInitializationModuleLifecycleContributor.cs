﻿namespace Fluxera.Extensions.Hosting.Modules
{
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class OnPreApplicationInitializationModuleLifecycleContributor : ModuleLifecycleContributorBase
	{
		public override void Initialize(IApplicationInitializationContext context, IModule module)
		{
			(module as IPreConfigureApplication)?.PreConfigure(context);
		}
	}
}
