namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;

	[PublicAPI]
	public abstract class ApplicationInitializationModule :
		IConfigureServicesModule,
		IApplicationInitializationModule,
		IApplicationShutdownModule
	{
		public virtual void PreConfigureServices(IServiceConfigurationContext context) { }

		public virtual void ConfigureServices(IServiceConfigurationContext context) { }

		public virtual void PostConfigureServices(IServiceConfigurationContext context) { }

		public virtual void PreConfigure(IApplicationInitializationContext context) { }

		public virtual void Configure(IApplicationInitializationContext context) { }

		public virtual void PostConfigure(IApplicationInitializationContext context) { }

		public virtual void OnApplicationShutdown(IApplicationShutdownContext context) { }
	}
}
