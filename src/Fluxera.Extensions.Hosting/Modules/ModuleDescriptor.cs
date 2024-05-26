namespace Fluxera.Extensions.Hosting.Modules
{
	using System;
	using System.Collections.Generic;
#if NET6_0
	using System.Collections.ObjectModel;
#endif
	using System.Reflection;

	internal sealed class ModuleDescriptor : IModuleDescriptor
	{
		private readonly IList<IModuleDescriptor> dependencies;

		public ModuleDescriptor(Type type, IModule instance, bool isLoadedAsPlugin)
		{
			Guard.ThrowIfNull(type);
			Guard.ThrowIfNull(instance);

			if(!type.GetTypeInfo().IsInstanceOfType(instance))
			{
				throw new ArgumentException(
					$"Given module instance ({instance.GetType().AssemblyQualifiedName}) is not an instance of given module type: {type.AssemblyQualifiedName}.");
			}

			this.Type = type;
			this.Assembly = type.Assembly;
			this.Instance = instance;
			this.IsLoadedAsPlugin = isLoadedAsPlugin;

			this.dependencies = new List<IModuleDescriptor>();
		}

		public Type Type { get; }

#if NET6_0
		public IReadOnlyCollection<IModuleDescriptor> Dependencies => new ReadOnlyCollection<IModuleDescriptor>(this.dependencies);
#endif

#if NET7_0_OR_GREATER
		public IReadOnlyCollection<IModuleDescriptor> Dependencies => this.dependencies.AsReadOnly();
#endif

		public Assembly Assembly { get; }

		public IModule Instance { get; }

		public bool IsLoadedAsPlugin { get; }

		public string Name => this.Assembly.FullName;

		public void AddDependency(IModuleDescriptor descriptor)
		{
			if(!this.dependencies.Contains(descriptor))
			{
				this.dependencies.Add(descriptor);
			}
		}

		public override string ToString()
		{
			return $"[ModuleDescriptor {this.Type.FullName}]";
		}
	}
}
