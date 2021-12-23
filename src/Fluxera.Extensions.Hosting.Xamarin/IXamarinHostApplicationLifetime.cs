namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;
	using Microsoft.Extensions.Hosting;

	/// <summary>
	///     A contract for a service that allows to perform execution of custom action
	///     during application sleep/resume cycle.
	/// </summary>
	[PublicAPI]
	public interface IXamarinHostApplicationLifetime : IHostApplicationLifetime
	{
		/// <summary>
		///     The registered actions are executed when the application has gone to sleep.
		/// </summary>
		ILifecycleRegister Sleeping { get; }

		/// <summary>
		///     The registered actions are executed when the application has resumed.
		/// </summary>
		ILifecycleRegister Resuming { get; }

		/// <summary>
		///     Notifies that the application is going to sleep by executing
		///     the registered action of the <see cref="Sleeping" />
		///     register.
		/// </summary>
		internal void NotifySleeping();

		/// <summary>
		///     Notifies that the application is going to sleep by executing
		///     the registered action of the <see cref="Resuming" />
		///     register.
		/// </summary>
		internal void NotifyResuming();
	}
}
