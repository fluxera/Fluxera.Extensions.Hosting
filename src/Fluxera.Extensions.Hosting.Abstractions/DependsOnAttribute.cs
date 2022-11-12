namespace Fluxera.Extensions.Hosting
{
	using System;
	using Fluxera.Extensions.Hosting.Modules;
	using JetBrains.Annotations;

	/// <summary>
	///     Defines a dependency of a modules. Can be used multiple times on a module class.
	/// </summary>
	[PublicAPI]
	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class DependsOnAttribute<TModule> : DependsOnAttribute
		where TModule : class, IModule
	{
		/// <summary>
		///     Creates a new instance of the <see cref="DependsOnAttribute" /> type.
		/// </summary>
		public DependsOnAttribute() : base(typeof(TModule))
		{
		}
	}

	/// <summary>
	///     Defines a dependency of a modules. Can be used multiple times on a module class.
	/// </summary>
	[PublicAPI]
	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class DependsOnAttribute : Attribute
	{
		/// <summary>
		///     Creates a new instance of the <see cref="DependsOnAttribute" /> type.
		/// </summary>
		/// <param name="dependentModuleType"></param>
		public DependsOnAttribute(Type dependentModuleType)
		{
			this.DependentModuleType = dependentModuleType;
		}

		/// <summary>
		///     The type of the module the decorated module depends on.
		/// </summary>
		public Type DependentModuleType { get; }
	}
}
