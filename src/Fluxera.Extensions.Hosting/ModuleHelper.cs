﻿namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using Fluxera.Extensions.Hosting.Modules;

	internal static class ModuleHelper
	{
		public static IEnumerable<Type> FindAllModuleTypes(Type startupModuleType)
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

		public static IEnumerable<Type> FindDependedModuleTypes(Type moduleType)
		{
			CheckModuleType(moduleType);

			IList<Type> dependencies = new List<Type>();

			IEnumerable<DependsOnAttribute> dependencyAttributes = moduleType.GetCustomAttributes<DependsOnAttribute>(true);
			foreach(DependsOnAttribute dependencyAttribute in dependencyAttributes)
			{
				if(!dependencies.Contains(dependencyAttribute.DependentModuleType))
				{
					dependencies.Add(dependencyAttribute.DependentModuleType);
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
