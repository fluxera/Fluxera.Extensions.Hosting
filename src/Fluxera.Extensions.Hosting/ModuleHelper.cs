namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	internal static class ModuleHelper
	{
		public static IList<Type> FindAllModuleTypes(Type startupModuleType)
		{
			IList<Type> moduleTypes = new List<Type>();
			AddModuleAndDependenciesRecursive(moduleTypes, startupModuleType);
			return moduleTypes;
		}

		private static void CheckModuleType(Type moduleType)
		{
			if(!IsModule(moduleType))
			{
				throw new ArgumentException($"Given type is not a module: {moduleType.AssemblyQualifiedName}.");
			}
		}

		public static IList<Type> FindDependedModuleTypes(Type moduleType)
		{
			CheckModuleType(moduleType);

			IList<Type> dependencies = new List<Type>();

			IEnumerable<IDependentTypesProvider> dependencyDescriptors = moduleType
				.GetCustomAttributes()
				.OfType<IDependentTypesProvider>();

			foreach(IDependentTypesProvider descriptor in dependencyDescriptors)
			{
				foreach(Type dependedModuleType in descriptor.GetDependentTypes())
				{
					if(!dependencies.Contains(dependedModuleType))
					{
						dependencies.Add(dependedModuleType);
					}
				}
			}

			return dependencies;
		}

		public static bool IsModule(Type type)
		{
			TypeInfo typeInfo = type.GetTypeInfo();

			return
				typeInfo.IsClass &&
				!typeInfo.IsAbstract &&
				!typeInfo.IsGenericType &&
				typeof(IModule).GetTypeInfo().IsAssignableFrom(type);
		}

		private static void AddModuleAndDependenciesRecursive(ICollection<Type> moduleTypes, Type moduleType)
		{
			CheckModuleType(moduleType);

			if(moduleTypes.Contains(moduleType))
			{
				return;
			}

			moduleTypes.Add(moduleType);

			foreach(Type dependedModuleType in FindDependedModuleTypes(moduleType))
			{
				AddModuleAndDependenciesRecursive(moduleTypes, dependedModuleType);
			}
		}

		internal static IList<Type> FindDependedModuleTypesRecursiveIncludingGivenModule(Type moduleType)
		{
			IList<Type> list = new List<Type>();
			AddModuleAndDependenciesRecursive(list, moduleType);
			return list;
		}
	}
}
