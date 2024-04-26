namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Collections.Generic;
	using Fluxera.Extensions.DependencyInjection;
	using Fluxera.Extensions.Hosting.Modules;
	using Fluxera.Extensions.Hosting.Plugins;
	using Fluxera.Utilities.Extensions;
	using Microsoft.Extensions.DependencyInjection;

	internal sealed class ModularApplicationBuilder : IModularApplicationBuilder
	{
		private readonly IServiceCollection services;
		private readonly Type startupModuleType;

		public ModularApplicationBuilder(
			Type startupModuleType,
			IServiceCollection services)
		{
			this.startupModuleType = startupModuleType;
			this.services = services;

			this.PluginSources = new PluginSourceList();
		}

		public IApplicationLoader Build(ApplicationLoaderBuilderFunc applicationLoaderFactory = null)
		{
			// Update configuration.
			this.services.UpdateConfiguration();

			// Add the lifecycle contributors.
			this.services.AddSingleton<IModuleLifecycleContributor, OnPreApplicationInitializationModuleLifecycleContributor>();
			this.services.AddSingleton<IModuleLifecycleContributor, OnApplicationInitializationModuleLifecycleContributor>();
			this.services.AddSingleton<IModuleLifecycleContributor, OnPostApplicationInitializationModuleLifecycleContributor>();
			this.services.AddSingleton<IModuleLifecycleContributor, OnApplicationShutdownModuleLifecycleContributor>();

			// Add the module manager.
			this.services.AddTransient<IModuleManager, ModuleManager>();

			// Add the module loader instance.
			this.services.AddSingleton<IModuleLoader>(new ModuleLoader());

			// Add logging services.
			this.services.AddLogging();

			// Add options services.
			this.services.AddOptions();

			// Initialize object accessor for the service provider.
			this.services.TryAddObjectAccessor<IServiceProvider>(ObjectAccessorLifetime.Configure);

			// Load the modules.
			IReadOnlyCollection<IModuleDescriptor> modules = this.LoadModules();

			// Create the application instance.
			IApplicationLoader applicationLoader = applicationLoaderFactory == null
				? new ApplicationLoader(this.startupModuleType, this.services, this.PluginSources, modules)
				: applicationLoaderFactory.Invoke(this.startupModuleType, this.services, this.PluginSources, modules);

			return applicationLoader;
		}

		public IPluginSourceList PluginSources { get; }

		private IReadOnlyCollection<IModuleDescriptor> LoadModules()
		{
			return this.services
				.GetSingletonInstance<IModuleLoader>()
				.LoadModules(this.startupModuleType, this.services, this.PluginSources)
				.AsReadOnly();
		}
	}
}
