namespace Fluxera.Extensions.Hosting
{
	using Fluxera.Extensions.Hosting.Plugins;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a service that configures the service and modules
	///     of the application and build the application loader instance.
	/// </summary>
	[PublicAPI]
	public interface IModularApplicationBuilder
	{
		/// <summary>
		///     Gets the plugin sources.
		/// </summary>
		IPluginSourceList PluginSources { get; }

		/// <summary>
		///     Builds the application loader using an optional loader factory function.
		/// </summary>
		/// <param name="applicationLoaderFactory"></param>
		/// <returns></returns>
		IApplicationLoader Build(ApplicationLoaderBuilderFunc applicationLoaderFactory = null);
	}
}
