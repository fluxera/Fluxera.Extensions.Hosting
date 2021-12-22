namespace Fluxera.Extensions.Hosting.Modules
{
	using JetBrains.Annotations;

	/// <summary>
	///     An abstract base class for modules that only support service configuration.
	/// </summary>
	[PublicAPI]
	public abstract class ConfigureServicesModule : IConfigureServicesModule
	{
		/// <inheritdoc />
		public virtual void PreConfigureServices(IServiceConfigurationContext context)
		{
		}

		/// <inheritdoc />
		public virtual void ConfigureServices(IServiceConfigurationContext context)
		{
		}

		/// <inheritdoc />
		public virtual void PostConfigureServices(IServiceConfigurationContext context)
		{
		}
	}
}
