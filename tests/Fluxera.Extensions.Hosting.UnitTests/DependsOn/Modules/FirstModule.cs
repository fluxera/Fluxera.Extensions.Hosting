namespace Fluxera.Extensions.Hosting.UnitTests.DependsOn.Modules
{
	using Fluxera.Extensions.Hosting.Modules;

	[DependsOn<SecondModule>]
	public class FirstModule : IModule
	{
	}
}
