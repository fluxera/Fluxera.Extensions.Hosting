namespace Fluxera.Extensions.Hosting.UnitTests.DependsOn
{
	using System.Collections.Generic;
	using System.Linq;
	using FluentAssertions;
	using Fluxera.Extensions.Hosting.Modules;
	using Fluxera.Extensions.Hosting.UnitTests.DependsOn.Modules;
	using NUnit.Framework;

	[TestFixture]
	public class DependsOnAttributeTests : StartupModuleTestBase<RootModule>
	{
		[Test]
		public void ShouldLoadAllFourModules()
		{
			IList<IModuleDescriptor> descriptors = this.ApplicationLoader.Modules.ToList();

			descriptors.Should().HaveCount(4);

			descriptors[3].Type.Should().Be<RootModule>();
			descriptors[2].Type.Should().Be<FirstModule>();
			descriptors[1].Type.Should().Be<SecondModule>();
			descriptors[0].Type.Should().Be<ThirdModule>();
		}
	}
}
