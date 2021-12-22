namespace Fluxera.Extensions.Hosting.Modules
{
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a module that supports the application shutdown action.
	/// </summary>
	[PublicAPI]
	public interface IApplicationShutdownModule : IOnApplicationShutdown
	{
	}
}
