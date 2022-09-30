namespace Fluxera.Extensions.Hosting.Modules
{
	using System.Collections.Generic;
	using System.Linq;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Logging;

	[UsedImplicitly]
	internal sealed class ModuleManager : IModuleManager
	{
		private readonly IModuleLifecycleContributor[] lifecycleContributors;
		private readonly ILogger<ModuleManager> logger;
		private readonly IModuleContainer moduleContainer;

		public ModuleManager(
			ILogger<ModuleManager> logger,
			IModuleContainer moduleContainer,
			IEnumerable<IModuleLifecycleContributor> lifecycleContributors)
		{
			this.moduleContainer = moduleContainer;
			this.logger = logger;

			// Get the lifecycle contributor services from the service provider.
			this.lifecycleContributors = lifecycleContributors.ToArray();
		}

		public void InitializeModules(IApplicationInitializationContext context)
		{
			this.LogListOfModules();

			foreach(IModuleLifecycleContributor contributor in this.lifecycleContributors)
			{
				foreach(IModuleDescriptor module in this.moduleContainer.Modules)
				{
					contributor.Initialize(context, module.Instance);
				}
			}

			this.logger.LogModulesInitialized();
		}

		public void ShutdownModules(IApplicationShutdownContext context)
		{
			IList<IModuleDescriptor> modules = this.moduleContainer.Modules.Reverse().ToList();

			foreach(IModuleLifecycleContributor contributor in this.lifecycleContributors)
			{
				foreach(IModuleDescriptor module in modules)
				{
					contributor.Shutdown(context, module.Instance);
				}
			}
		}

		private void LogListOfModules()
		{
			foreach(IModuleDescriptor module in this.moduleContainer.Modules)
			{
				this.logger.LogLoadedModule(module.Type.FullName);
			}
		}
	}
}
