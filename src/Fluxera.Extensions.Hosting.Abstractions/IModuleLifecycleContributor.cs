namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a class that delegates the application lifecycle
	///     actions to the given modules.
	/// </summary>
	[PublicAPI]
	public interface IModuleLifecycleContributor
	{
		/// <summary>
		///     This method is called when the application initialization starts.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="module"></param>
		void Initialize(IApplicationInitializationContext context, IModule module);

		/// <summary>
		///     This method is called when the application shuts down.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="module"></param>
		void Shutdown(IApplicationShutdownContext context, IModule module);
	}
}
