namespace Fluxera.Extensions.Hosting.UnitTests
{
	using System.Linq;
	using FluentAssertions;
	using Fluxera.Extensions.Hosting.Modules;
	using NUnit.Framework;

	[TestFixture]
	public class HostingModuleTests : StartupModuleTestBase<TestApplicationModule>
	{
		[Test]
		public void ShouldCallConfigure()
		{
			IModuleDescriptor descriptor = this.ApplicationLoader.Modules.First();
			TestApplicationModule module = descriptor.Instance as TestApplicationModule;

			module.Should().NotBeNull();
			module!.ConfigureWasCalled.Should().BeTrue();
		}

		[Test]
		public void ShouldCallConfigureServices()
		{
			IModuleDescriptor descriptor = this.ApplicationLoader.Modules.First();
			TestApplicationModule module = descriptor.Instance as TestApplicationModule;

			module.Should().NotBeNull();
			module!.ConfigureServicesWasCalled.Should().BeTrue();
		}

		[Test]
		public void ShouldCallOnApplicationShutdown()
		{
			this.ApplicationLoader.Shutdown();
			IModuleDescriptor descriptor = this.ApplicationLoader.Modules.First();
			TestApplicationModule module = descriptor.Instance as TestApplicationModule;

			module.Should().NotBeNull();
			module!.OnApplicationShutdownWasCalled.Should().BeTrue();
		}

		[Test]
		public void ShouldCallPostConfigure()
		{
			IModuleDescriptor descriptor = this.ApplicationLoader.Modules.First();
			TestApplicationModule module = descriptor.Instance as TestApplicationModule;

			module.Should().NotBeNull();
			module!.PostConfigureWasCalled.Should().BeTrue();
		}

		[Test]
		public void ShouldCallPostConfigureServices()
		{
			IModuleDescriptor descriptor = this.ApplicationLoader.Modules.First();
			TestApplicationModule module = descriptor.Instance as TestApplicationModule;

			module.Should().NotBeNull();
			module!.PostConfigureServicesWasCalled.Should().BeTrue();
		}

		[Test]
		public void ShouldCallPreConfigure()
		{
			IModuleDescriptor descriptor = this.ApplicationLoader.Modules.First();
			TestApplicationModule module = descriptor.Instance as TestApplicationModule;

			module.Should().NotBeNull();
			module!.PreConfigureWasCalled.Should().BeTrue();
		}

		[Test]
		public void ShouldCallPreConfigureServices()
		{
			IModuleDescriptor descriptor = this.ApplicationLoader.Modules.First();
			TestApplicationModule module = descriptor.Instance as TestApplicationModule;

			module.Should().NotBeNull();
			module!.PreConfigureServicesWasCalled.Should().BeTrue();
		}
	}
}
