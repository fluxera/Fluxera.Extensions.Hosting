namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IConfigureServices : IModule
	{
		void ConfigureServices(IServiceConfigurationContext context);
	}
}