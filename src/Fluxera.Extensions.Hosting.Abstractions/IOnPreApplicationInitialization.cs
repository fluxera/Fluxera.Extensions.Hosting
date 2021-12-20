namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IOnPreApplicationInitialization : IModule
	{
		void PreConfigure(IApplicationInitializationContext context);
	}
}