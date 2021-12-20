namespace Fluxera.Extensions.Hosting
{
	internal static class ApplicationLoaderBuilderFuncFactory
	{
		public static readonly ApplicationLoaderBuilderFunc CreateApplication = (startupModuleType, services, pluginSources, modules) =>
		{
			IApplicationLoader application = new WebApplicationLoader(startupModuleType, services, pluginSources, modules);
			return application;
		};
	}
}
