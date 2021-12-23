namespace Fluxera.Extensions.Hosting.Modules
{
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a module that supports the application shutdown action.
	/// </summary>
	[PublicAPI]
	public interface IShutdownApplicationModule : IModule
	{
		/// <summary>
		///     This method is called when the application shuts down.
		/// </summary>
		/// <param name="context"></param>
		void OnApplicationShutdown(IApplicationShutdownContext context);
	}
}
