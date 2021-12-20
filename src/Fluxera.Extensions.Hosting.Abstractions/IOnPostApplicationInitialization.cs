namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IOnPostApplicationInitialization : IModule
	{
		void PostConfigure(IApplicationInitializationContext context);
	}
}