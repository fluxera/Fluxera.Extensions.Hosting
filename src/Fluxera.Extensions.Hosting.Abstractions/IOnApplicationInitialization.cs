namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IOnApplicationInitialization : IModule
	{
		void Configure(IApplicationInitializationContext context);
	}
}