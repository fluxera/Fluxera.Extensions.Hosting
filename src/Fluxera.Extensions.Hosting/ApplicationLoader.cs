namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Fluxera.Extensions.DependencyInjection;
	using Fluxera.Extensions.Hosting.Modules;
	using Fluxera.Extensions.Hosting.Plugins;
	using Fluxera.Guards;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;

	public class ApplicationLoader : IApplicationLoader
	{
		private bool isShutDown;

		public ApplicationLoader(
			Type startupModuleType,
			IServiceCollection services,
			IPluginSourceList pluginSources,
			IReadOnlyCollection<IModuleDescriptor> modules)
		{
			Guard.Against.Null(startupModuleType, nameof(startupModuleType));
			Guard.Against.Null(services, nameof(services));
			Guard.Against.Null(pluginSources, nameof(pluginSources));
			Guard.Against.Null(modules, nameof(modules));

			this.StartupModuleType = startupModuleType;
			this.Services = services;
			this.PluginSources = pluginSources;
			this.Modules = modules;

			services.AddSingleton<IModuleContainer>(this);

			// Configure the services of the modules.
			modules.ConfigureServices(services);

			services.AddSingleton<IApplicationLoader>(this);
		}

		public Type StartupModuleType { get; }

		public IServiceProvider ServiceProvider { get; private set; }

		public IReadOnlyCollection<IModuleDescriptor> Modules { get; }

		public IServiceCollection Services { get; }

		public IConfiguration Configuration => this.Services.GetSingletonInstance<IConfiguration>();

		public IPluginSourceList PluginSources { get; }

		/// <inheritdoc />
		public virtual void Initialize(IApplicationLoaderInitializationContext context)
		{
			Guard.Against.Null(context, nameof(context));

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

		protected virtual IApplicationInitializationContext CreateApplicationInitializationContext(
			IServiceProvider serviceProvider)
		{
			return new ApplicationInitializationContext(serviceProvider);
		}
	}
}
