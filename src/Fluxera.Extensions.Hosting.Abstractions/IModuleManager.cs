namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for manager service that controls the startup and
	///     shutdown of the application's modules.
	/// </summary>
	[PublicAPI]
	public interface IModuleManager
	{
		/// <summary>
		///     Initialize all available modules.
		/// </summary>
		/// <param name="context"></param>
		void InitializeModules(IApplicationInitializationContext context);

		/// <summary>
		///     Shutdown all available modules.
		/// </summary>
		/// <param name="context"></param>
		void ShutdownModules(IApplicationShutdownContext context);
	}
}
