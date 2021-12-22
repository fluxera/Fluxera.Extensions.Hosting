namespace Fluxera.Extensions.Hosting.Modules
{
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a module that supports the complete application initialization pipeline.
	/// </summary>
	[PublicAPI]
	public interface IApplicationInitializationModule : IOnPreApplicationInitialization, IOnApplicationInitialization, IOnPostApplicationInitialization
	{
	}
}
