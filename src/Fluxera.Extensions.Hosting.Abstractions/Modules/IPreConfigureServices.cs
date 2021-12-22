namespace Fluxera.Extensions.Hosting.Modules
{
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a module that supports the pre-configure services action.
	/// </summary>
	[PublicAPI]
	public interface IPreConfigureServices : IModule
	{
		/// <summary>
		///     Executed before the actual configure services action.
		/// </summary>
		/// <param name="context">The service configuration context.</param>
		void PreConfigureServices(IServiceConfigurationContext context);
	}
}
