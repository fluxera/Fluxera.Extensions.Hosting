namespace Fluxera.Extensions.Hosting.UnitTests
{
	using System;
	using System.Linq;
	using FluentAssertions;
	using Fluxera.Extensions.Hosting.Modules;
	using NUnit.Framework;

	[TestFixture]
	public class ApplicationLoaderServicesTests : StartupModuleTestBase<TestApplicationModule>
	{
		[Test]
		public void ShouldHaveModuleLoader()
		{
			IModuleDescriptor descriptor = this.ApplicationLoader.Modules.First();
			TestApplicationModule module = descriptor.Instance as TestApplicationModule;

			module.HasModuleContainer.Should().BeTrue();
		}

		[Test]
		public void ShouldNotAllowCallingConfigureServicesMultipleTimes()
		{
			Action action = () =>
			{
				this.ApplicationLoader.ConfigureServices();
			};

			action.Should().ThrowExactly<InvalidOperationException>();
		}
	}
}
