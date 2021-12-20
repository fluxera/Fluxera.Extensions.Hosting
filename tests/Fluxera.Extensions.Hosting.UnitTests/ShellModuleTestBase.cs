namespace Fluxera.Extensions.Hosting.UnitTests
{
	using System;
	using DependencyInjection;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Hosting.Internal;
	using NUnit.Framework;

	[PublicAPI]
	public abstract class ShellModuleTestBase<TStartupModule> : TestBaseWithServiceProvider
		where TStartupModule : class, IModule
	{
		protected IApplicationLoader Application { get; private set; }

		protected override IServiceProvider ServiceProvider => this.Application.ServiceProvider;

		protected IServiceProvider RootServiceProvider { get; private set; }

		protected IServiceScope TestServiceScope { get; private set; }

		[SetUp]
		public void Setup()
		{
			// TODO
			//IServiceCollection services = this.CreateServiceCollection();
			//IConfiguration configuration = new ConfigurationBuilder().Build();

			//this.BeforeAddApplication(services);

			//IHostEnvironment environment = new HostingEnvironment
			//{
			//	EnvironmentName = "Development",
			//	ApplicationName = "UnitTests",
			//};
			//services.AddObjectAccessor(environment, ObjectAccessorLifetime.ConfigureServices);

			//services.AddApplicationLoader<TStartupModule>(
			//	configuration,
			//	environment,
			//	null,
			//	null);

			//this.AfterAddApplication(services);

			//this.RootServiceProvider = this.CreateServiceProvider(services);
			//this.TestServiceScope = this.RootServiceProvider.CreateScope();

			//this.Application = this.RootServiceProvider.GetRequiredService<IApplicationLoader>();
			//this.Application.Initialize(this.TestServiceScope.ServiceProvider);

			//this.AfterSetup();
		}

		[TearDown]
		public void TearDown()
		{
			this.Application?.Shutdown();
			this.TestServiceScope?.Dispose();

			this.Application = null;
			this.TestServiceScope = null;
			this.RootServiceProvider = null;
		}

		protected virtual IServiceCollection CreateServiceCollection()
		{
			return new ServiceCollection();
		}

		protected virtual void BeforeAddApplication(IServiceCollection services) { }

		protected virtual void AfterAddApplication(IServiceCollection services) { }

		protected virtual IServiceProvider CreateServiceProvider(IServiceCollection services)
		{
			return services.BuildServiceProvider();
		}

		protected virtual void AfterSetup() { }
	}
}
