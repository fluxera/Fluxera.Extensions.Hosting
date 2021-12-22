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
	public sealed class DependsOnAttribute : Attribute
	{
		/// <summary>
		///     Creates a new instance of the <see cref="DependsOnAttribute" /> type.
		/// </summary>
		/// <param name="dependentType"></param>
		public DependsOnAttribute(Type dependentType)
		{
			this.DependentType = dependentType;
		}

		/// <summary>
		///     The type of the module the decorated module depends on.
		/// </summary>
		public Type DependentType { get; }
	}
}
