namespace Fluxera.Extensions.Hosting
{
	using System;
	using JetBrains.Annotations;

	/// <summary>
	///     Handles registration of events.
	/// </summary>
	[PublicAPI]
	public interface ILifecycleRegister
	{
		/// <summary>
		///     Registers a given action.
		/// </summary>
		/// <param name="action">The action to be registered.</param>
		void Register(Action action);
	}
}
