namespace Fluxera.Extensions.Hosting.Modules
{
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a module that supports the configure application action.
	/// </summary>
	[PublicAPI]
	public interface IOnApplicationInitialization : IModule
	{
		/// <summary>
		///     This method is called after the pre-configure and before the
		///     post-configure actions.
		/// </summary>
		/// <param name="context"></param>
		void Configure(IApplicationInitializationContext context);
	}
}
