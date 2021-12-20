namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IApplicationInitializationModule : 
		IOnPreApplicationInitialization, 
		IOnApplicationInitialization,
		IOnPostApplicationInitialization
	{

	}
}
