namespace Fluxera.Extensions.Hosting.UnitTests
{
	using Fluxera.Extensions.DependencyInjection;
	using Fluxera.Extensions.Hosting.Modules;
	using JetBrains.Annotations;

	[PublicAPI]
	public class TestApplicationModule : ConfigureApplicationModule
	{
		public bool PreConfigureServicesWasCalled { get; set; }

		public bool ConfigureServicesWasCalled { get; set; }

		public bool PostConfigureServicesWasCalled { get; set; }

		public bool PreConfigureWasCalled { get; set; }

		public bool ConfigureWasCalled { get; set; }

		public bool PostConfigureWasCalled { get; set; }

		public bool OnApplicationShutdownWasCalled { get; set; }

		public bool HasModuleContainer { get; set; }

		/// <inheritdoc />
		public override void PreConfigureServices(IServiceConfigurationContext context)
		{
			this.PreConfigureServicesWasCalled = true;
		}

		/// <inheritdoc />
		public override void ConfigureServices(IServiceConfigurationContext context)
		{
			this.ConfigureServicesWasCalled = true;

			IModuleContainer moduleLoader = context.Services.GetObjectOrDefault<IModuleContainer>();
			this.HasModuleContainer = moduleLoader != null;
		}

		/// <inheritdoc />
		public override void PostConfigureServices(IServiceConfigurationContext context)
		{
			this.PostConfigureServicesWasCalled = true;
		}

		/// <inheritdoc />
		public override void PreConfigure(IApplicationInitializationContext context)
		{
			this.PreConfigureWasCalled = true;
		}

		/// <inheritdoc />
		public override void Configure(IApplicationInitializationContext context)
		{
			this.ConfigureWasCalled = true;
		}

		/// <inheritdoc />
		public override void PostConfigure(IApplicationInitializationContext context)
		{
			this.PostConfigureWasCalled = true;
		}

		/// <inheritdoc />
		public override void OnApplicationShutdown(IApplicationShutdownContext context)
		{
			this.OnApplicationShutdownWasCalled = true;
		}
	}
}
