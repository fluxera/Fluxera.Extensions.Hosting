namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IConfigureServicesModule : 
		IPreConfigureServices, 
		IConfigureServices, 
		IPostConfigureServices
	{
	}
}
