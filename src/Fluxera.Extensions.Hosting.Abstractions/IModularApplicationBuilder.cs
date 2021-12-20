namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IModularApplicationBuilder
	{
		IApplicationLoader Build(ApplicationLoaderBuilderFunc? applicationLoaderFactory = null);

		IPluginSourceList PluginSources { get; }
	}
}
