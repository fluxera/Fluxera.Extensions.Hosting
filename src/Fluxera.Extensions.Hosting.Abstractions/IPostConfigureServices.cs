namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IPostConfigureServices : IModule
	{
		void PostConfigureServices(IServiceConfigurationContext context);
	}
}