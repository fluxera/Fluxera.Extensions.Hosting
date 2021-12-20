namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IModuleManager
	{
		void InitializeModules(IApplicationInitializationContext context);

		void ShutdownModules(IApplicationShutdownContext context);
	}
}
