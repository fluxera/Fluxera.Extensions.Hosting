namespace Fluxera.Extensions.Hosting.UnitTests
{
	using JetBrains.Annotations;

	[PublicAPI]
	public class TestModuleBase : ApplicationInitializationModule
	{
		public bool PreConfigureServicesIsCalled { get; set; }

		public bool ConfigureServicesIsCalled { get; set; }

		public bool PostConfigureServicesIsCalled { get; set; }

		public bool OnApplicationInitializeIsCalled { get; set; }

		public bool OnApplicationShutdownIsCalled { get; set; }

		public override void PreConfigureServices(IServiceConfigurationContext context)
		{
			this.PreConfigureServicesIsCalled = true;
		}

		public override void ConfigureServices(IServiceConfigurationContext context)
		{
			this.ConfigureServicesIsCalled = true;
		}

		public override void PostConfigureServices(IServiceConfigurationContext context)
		{
			this.PostConfigureServicesIsCalled = true;
		}

		public override void Configure(IApplicationInitializationContext context)
		{
			this.OnApplicationInitializeIsCalled = true;
		}

		public override void OnApplicationShutdown(IApplicationShutdownContext context)
		{
			this.OnApplicationShutdownIsCalled = true;
		}
	}
}
