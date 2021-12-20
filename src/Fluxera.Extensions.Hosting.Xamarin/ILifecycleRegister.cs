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
		///     Registers a given callback.
		/// </summary>
		/// <param name="callback">The callback to be registered.</param>
		void Register(Action callback);
	}
}
