namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Fluxera.Extensions.DependencyInjection;
	using Fluxera.Extensions.Hosting.Modules;
	using Fluxera.Extensions.Hosting.Plugins;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;

	/// <summary>
	///     An application loader service.
	/// </summary>
	public class ApplicationLoader : IApplicationLoader
	{
		private bool isConfigured;
		private bool isShutDown;

		/// <summary>
		///     Creates a new instance of the <see cref="ApplicationLoader" /> type.
		/// </summary>
		/// <param name="startupModuleType">The startup module type.</param>
		/// <param name="services">The service collection.</param>
		/// <param name="pluginSources">The plugin sources.</param>
		/// <param name="modules">The modules.</param>
		public ApplicationLoader(
			Type startupModuleType,
			IServiceCollection services,
			IPluginSourceList pluginSources,
			IReadOnlyCollection<IModuleDescriptor> modules)
		{
			Guard.ThrowIfNull(startupModuleType);
			Guard.ThrowIfNull(services);
			Guard.ThrowIfNull(pluginSources);
			Guard.ThrowIfNull(modules);

			this.StartupModuleType = startupModuleType;
			this.Services = services;
			this.PluginSources = pluginSources;
			this.Modules = modules;
		}

		/// <inheritdoc />
		public Type StartupModuleType { get; }

		/// <inheritdoc />
		public IServiceProvider ServiceProvider { get; private set; }

		/// <inheritdoc />
		public IReadOnlyCollection<IModuleDescriptor> Modules { get; }

		/// <inheritdoc />
		public IServiceCollection Services { get; }

		/// <inheritdoc />
		public IConfiguration Configuration => this.Services.GetSingletonInstance<IConfiguration>();

		/// <inheritdoc />
		public IPluginSourceList PluginSources { get; }

		/// <inheritdoc />
		public void ConfigureServices()
		{
			if(this.isConfigured)
			{
				throw new InvalidOperationException("The application loader can only configure the services once.");
			}

			// Configure the services of the modules.
			this.Modules.ConfigureServices(this.Services);
			this.isConfigured = true;
		}

		/// <inheritdoc />
		public virtual void Initialize(IApplicationLoaderInitializationContext context)
		{
			Guard.ThrowIfNull(context);

			this.ServiceProvider = context.ServiceProvider;

			// Initialize the modules.
			this.ServiceProvider
				.GetRequiredService<IModuleManager>()
				.InitializeModules(this.CreateApplicationInitializationContext(this.ServiceProvider));

			// Dispose all "Configure" object accessors.
			IEnumerable<IObjectAccessor> objectAccessors = this.ServiceProvider
				.GetServices<IObjectAccessor>()
				.Where(x => x.Context == ObjectAccessorLifetime.Configure);

			foreach(IObjectAccessor objectAccessor in objectAccessors)
			{
				objectAccessor.Dispose();
			}

			IHostApplicationLifetime hostApplicationLifetime = this.ServiceProvider.GetRequiredService<IHostApplicationLifetime>();
			hostApplicationLifetime.ApplicationStopping.Register(this.Shutdown);
		}

		/// <inheritdoc />
		public void Shutdown()
		{
			this.isShutDown = true;

			using(IServiceScope scope = this.ServiceProvider.CreateScope())
			{
				scope.ServiceProvider
					.GetRequiredService<IModuleManager>()
					.ShutdownModules(new ApplicationShutdownContext(scope.ServiceProvider));
			}
		}

		/// <inheritdoc />
		public void Dispose()
		{
			if(!this.isShutDown)
			{
				this.Shutdown();
			}
		}

		/// <summary>
		///     Creates the <see cref="IApplicationInitializationContext" /> instance to use.
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <returns></returns>
		protected virtual IApplicationInitializationContext CreateApplicationInitializationContext(IServiceProvider serviceProvider)
		{
			return new ApplicationInitializationContext(serviceProvider);
		}
	}
}
