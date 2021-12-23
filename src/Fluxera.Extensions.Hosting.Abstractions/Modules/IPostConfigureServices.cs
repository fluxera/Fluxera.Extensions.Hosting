namespace Fluxera.Extensions.Hosting.Modules
{
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a module that supports the post-configure services action.
	/// </summary>
	[PublicAPI]
	public interface IPostConfigureServices : IModule
	{
		/// <summary>
		///     Executed after the actual configure services action.
		/// </summary>
		/// <param name="context">The service configuration context.</param>
		void PostConfigureServices(IServiceConfigurationContext context);
	}
}
