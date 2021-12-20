namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using Fluxera.Guards;
	using Fluxera.Utilities.Extensions;

	internal sealed class ModuleDescriptor : IModuleDescriptor
	{
		private readonly IList<IModuleDescriptor> dependencies;

		public ModuleDescriptor(Type type, IModule instance, bool isLoadedAsPlugin)
		{
			Guard.Against.Null(type, nameof(type));
			Guard.Against.Null(instance, nameof(instance));

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

		public IReadOnlyCollection<IModuleDescriptor> Dependencies => this.dependencies.AsReadOnly();

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
