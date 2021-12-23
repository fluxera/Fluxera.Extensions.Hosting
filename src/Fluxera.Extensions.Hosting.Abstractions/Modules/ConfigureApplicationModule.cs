namespace Fluxera.Extensions.Hosting.Modules
{
	using JetBrains.Annotations;

	/// <summary>
	///     An abstract base class for implementing module classes that supports the
	///     configure services actions and also the application initialization and
	///     shutdown actions.
	/// </summary>
	[PublicAPI]
	public abstract class ConfigureApplicationModule : IConfigureServicesModule, IConfigureApplicationModule, IShutdownApplicationModule
	{
		/// <inheritdoc />
		public virtual void PreConfigure(IApplicationInitializationContext context)
		{
		}

		/// <inheritdoc />
		public virtual void Configure(IApplicationInitializationContext context)
		{
		}

		/// <inheritdoc />
		public virtual void PostConfigure(IApplicationInitializationContext context)
		{
		}

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

		/// <inheritdoc />
		public virtual void OnApplicationShutdown(IApplicationShutdownContext context)
		{
		}
	}
}
