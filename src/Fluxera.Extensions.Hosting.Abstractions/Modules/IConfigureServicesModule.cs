namespace Fluxera.Extensions.Hosting.Modules
{
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a module that supports the complete service configuration pipeline.
	/// </summary>
	[PublicAPI]
	public interface IConfigureServicesModule : IPreConfigureServices, IConfigureServices, IPostConfigureServices
	{
	}
}
