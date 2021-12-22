namespace Fluxera.Extensions.Hosting.Modules
{
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a module that supports the configure services action.
	/// </summary>
	[PublicAPI]
	public interface IConfigureServices : IModule
	{
		/// <summary>
		///     Configures the service of teh module. It is executed after the
		///     pre-configure und before the post-configure service actions.
		/// </summary>
		/// <param name="context">The service configuration context.</param>
		void ConfigureServices(IServiceConfigurationContext context);
	}
}
