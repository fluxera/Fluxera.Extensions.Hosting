namespace Fluxera.Extensions.Hosting.UnitTests
{
	using System.Linq;
	using FluentAssertions;
	using Fluxera.Extensions.Hosting.Modules;
	using NUnit.Framework;

	[TestFixture]
	public class HostingGenericModuleTests : StartupModuleTestBase<GenericApplicationModule<string>>
	{
		[Test]
		public void ShouldLoadModule()
		{
			IModuleDescriptor descriptor = this.ApplicationLoader.Modules.First();
			descriptor.Instance.Should().BeOfType<GenericApplicationModule<string>>();
		}
	}
}
