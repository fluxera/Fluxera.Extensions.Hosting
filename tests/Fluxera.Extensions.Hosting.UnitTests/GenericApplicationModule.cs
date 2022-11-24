namespace Fluxera.Extensions.Hosting.UnitTests
{
	using Fluxera.Extensions.Hosting.Modules;
	using JetBrains.Annotations;

	[PublicAPI]
	public class GenericApplicationModule<T> : ConfigureApplicationModule
	{
	}
}
