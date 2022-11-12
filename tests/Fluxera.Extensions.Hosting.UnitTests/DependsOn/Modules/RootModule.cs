namespace Fluxera.Extensions.Hosting.UnitTests.DependsOn.Modules
{
	using Fluxera.Extensions.Hosting.Modules;

	[DependsOn(typeof(FirstModule))]
	public class RootModule : IModule
	{
	}
}
