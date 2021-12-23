namespace Fluxera.Extensions.Hosting
{
	using System;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a class that handles registration of actions.
	/// </summary>
	[PublicAPI]
	public interface ILifecycleRegister
	{
		/// <summary>
		///     Registers the given action.
		/// </summary>
		/// <param name="action">The action to be registered.</param>
		void Register(Action action);

		/// <summary>
		///     Executes all registered action of this register.
		/// </summary>
		void Notify();
	}
}
