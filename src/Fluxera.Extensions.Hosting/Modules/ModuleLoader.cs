namespace Fluxera.Extensions.Hosting.Modules
{
	using System;
	using System.Collections.Generic;
#if NET6_0
	using System.Collections.ObjectModel;
#endif
	using System.Linq;
	using Fluxera.Extensions.Hosting.Plugins;
	using Microsoft.Extensions.DependencyInjection;

	internal sealed class ModuleLoader : IModuleLoader
	{
		public IReadOnlyCollection<IModuleDescriptor> LoadModules(
			Type startupModuleType,
			IServiceCollection services,
			IPluginSourceList pluginSources)
		{
			Guard.ThrowIfNull(startupModuleType);
			Guard.ThrowIfNull(services);
			Guard.ThrowIfNull(pluginSources);

			IList<IModuleDescriptor> modules = GetModuleDescriptors(services, startupModuleType, pluginSources);
			modules = SortByDependency(modules, startupModuleType);

#if NET6_0
			return new ReadOnlyCollection<IModuleDescriptor>(modules);
#endif
#if NET7_0_OR_GREATER
			return modules.AsReadOnly();
#endif
		}

		private static IList<IModuleDescriptor> GetModuleDescriptors(
			IServiceCollection services,
			Type startupModuleType,
			IPluginSourceList pluginSources)
		{
			IList<IModuleDescriptor> modules = new List<IModuleDescriptor>();
			FillModules(modules, services, startupModuleType, pluginSources);
			SetDependencies(modules);

			return modules;
		}

		private static void FillModules(
			ICollection<IModuleDescriptor> modules,
			IServiceCollection services,
			Type startupModuleType,
			IPluginSourceList pluginSources)
		{
			// All modules starting from the startup module.
			foreach(Type moduleType in ModuleHelper.FindAllModuleTypes(startupModuleType))
			{
				modules.Add(CreateModuleDescriptor(services, moduleType));
			}

			// Plugin modules.
			foreach(Type moduleType in pluginSources.GetAllModules())
			{
				if(modules.Any(m => m.Type == moduleType))
				{
					continue;
				}

				modules.Add(CreateModuleDescriptor(services, moduleType, true));
			}
		}

		private static void SetDependencies(IList<IModuleDescriptor> moduleDescriptors)
		{
			foreach(IModuleDescriptor module in moduleDescriptors)
			{
				SetDependencies(moduleDescriptors, module);
			}
		}

		private static IList<IModuleDescriptor> SortByDependency(ICollection<IModuleDescriptor> modules,
			Type startupModuleType)
		{
			IList<IModuleDescriptor> sortedModules = modules.SortByDependencies(m => m.Dependencies);
			sortedModules.MoveItem(m => m.Type == startupModuleType, modules.Count - 1);
			return sortedModules;
		}

		private static IModuleDescriptor CreateModuleDescriptor(IServiceCollection services, Type moduleType,
			bool isLoadedAsPlugin = false)
		{
			IModule module = CreateAndRegisterModule(services, moduleType);
			return new ModuleDescriptor(moduleType, module, isLoadedAsPlugin);
		}

		private static IModule CreateAndRegisterModule(IServiceCollection services, Type moduleType)
		{
			IModule module = (IModule)Activator.CreateInstance(moduleType);
			services.AddSingleton(module);
			services.AddSingleton(moduleType, module);
			return module;
		}

		private static void SetDependencies(IList<IModuleDescriptor> moduleDescriptors,
			IModuleDescriptor moduleDescriptor)
		{
			foreach(Type dependedModuleType in ModuleHelper.FindDependedModuleTypes(moduleDescriptor.Type))
			{
				IModuleDescriptor dependedModule = moduleDescriptors.FirstOrDefault(descriptor => descriptor.Type == dependedModuleType);
				if(dependedModule == null)
				{
					throw new Exception($"Could not find a dependent module '{dependedModuleType.Name}' for module type '{moduleDescriptor.Type.Namespace}'.");
				}

				ModuleDescriptor moduleDescriptorEx = (ModuleDescriptor)moduleDescriptor;
				moduleDescriptorEx.AddDependency(dependedModule);
			}
		}
	}
}
