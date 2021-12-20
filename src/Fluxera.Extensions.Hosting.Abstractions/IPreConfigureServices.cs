namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IPreConfigureServices : IModule
	{
		void PreConfigureServices(IServiceConfigurationContext context);
	}
}
