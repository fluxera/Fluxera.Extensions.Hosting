namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;

	[PublicAPI]
	public abstract class ConfigureServicesModule : IConfigureServicesModule
	{
		public virtual void PreConfigureServices(IServiceConfigurationContext context)
		{
		}

		public virtual void ConfigureServices(IServiceConfigurationContext context)
		{
		}

		public virtual void PostConfigureServices(IServiceConfigurationContext context)
		{
		}
	}
}
