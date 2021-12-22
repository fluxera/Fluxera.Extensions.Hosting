namespace Fluxera.Extensions.Hosting.Modules
{
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a module that supports the pre-configure action.
	/// </summary>
	[PublicAPI]
	public interface IOnPreApplicationInitialization : IModule
	{
		/// <summary>
		///     This method is called before the actual configure action.
		/// </summary>
		/// <param name="context"></param>
		void PreConfigure(IApplicationInitializationContext context);
	}
}
