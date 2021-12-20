namespace Fluxera.Extensions.Hosting
{
	using System;
	using JetBrains.Annotations;

	/// <summary>
	///     Used to define dependencies of a type.
	/// </summary>
	[PublicAPI]
	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public sealed class DependsOnAttribute : Attribute, IDependentTypesProvider
	{
		public DependsOnAttribute(Type dependentType)
		{
			this.DependentType = dependentType;
		}

		public Type DependentType { get; }

		public Type[] GetDependentTypes()
		{
			return new Type[] { this.DependentType };
		}
	}
}
