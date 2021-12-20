namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;
	using Microsoft.Extensions.Options;

	[UsedImplicitly]
	internal sealed class ModuleManager : IModuleManager
	{
		private readonly IModuleLifecycleContributor[] lifecycleContributors;
		private readonly ILogger<ModuleManager> logger;
		private readonly IModuleContainer moduleContainer;

		public ModuleManager(
			ILogger<ModuleManager> logger,
			IModuleContainer moduleContainer,
			IOptions<ModuleLifecycleOptions> options,
			IServiceProvider serviceProvider)
		{
			this.moduleContainer = moduleContainer;
			this.logger = logger;

			this.lifecycleContributors = options.Value
				.Contributors
				.Select(serviceProvider.GetRequiredService)
				.Cast<IModuleLifecycleContributor>()
				.ToArray();
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

			this.logger.LogDebug("Initialized all modules.");
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
			string message = $"Loaded modules:{Environment.NewLine}";

			foreach(IModuleDescriptor module in this.moduleContainer.Modules)
			{
				message += $"\t- {module.Type.FullName}{Environment.NewLine}";
			}

			this.logger.LogInformation(message.TrimEnd(Environment.NewLine.ToCharArray()));
		}
	}
}
