namespace Fluxera.Extensions.Hosting.Modules
{
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a module that supports the post-configure action.
	/// </summary>
	[PublicAPI]
	public interface IPostConfigureApplication : IModule
	{
		/// <summary>
		///     This method is called after the actual configure action.
		/// </summary>
		/// <param name="context"></param>
		void PostConfigure(IApplicationInitializationContext context);
	}
}
