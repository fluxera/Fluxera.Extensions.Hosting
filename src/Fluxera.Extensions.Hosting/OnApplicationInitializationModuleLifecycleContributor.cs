﻿namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class OnApplicationInitializationModuleLifecycleContributor : ModuleLifecycleContributorBase
	{
		public override void Initialize(IApplicationInitializationContext context, IModule module)
		{
			(module as IOnApplicationInitialization)?.Configure(context);
		}
	}
}