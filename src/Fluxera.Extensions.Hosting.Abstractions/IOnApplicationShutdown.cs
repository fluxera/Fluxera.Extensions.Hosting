namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IOnApplicationShutdown : IModule
	{
		void OnApplicationShutdown(IApplicationShutdownContext context);
	}
}