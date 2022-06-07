namespace Fluxera.Extensions.Hosting.UnitTests
{
	using System;
	using Fluxera.Extensions.Hosting.Modules;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Hosting.Internal;
	using NUnit.Framework;

	[PublicAPI]
	public abstract class StartupModuleTestBase<TStartupModule> : TestBase
		where TStartupModule : class, IModule
	{
		protected IApplicationLoader ApplicationLoader { get; private set; }

		[SetUp]
		public void Setup()
		{
			IServiceProvider serviceProvider = BuildServiceProvider(services =>
			{
				IConfiguration configuration = new ConfigurationBuilder().Build();
				IHostEnvironment environment = new HostingEnvironment
				{
					EnvironmentName = "Development",
					ApplicationName = "UnitTests",
				};

				services.AddSingleton(environment);
				services.AddSingleton<IHostLifetime, TestLifetime>();
				services.AddSingleton<IHostApplicationLifetime, TestApplicationLifetime>();

				services.AddApplicationLoader<TStartupModule>(configuration, environment, CreateBootstrapperLogger());
			});

			this.ApplicationLoader = serviceProvider.GetRequiredService<IApplicationLoader>();
			this.ApplicationLoader.Initialize(new ApplicationLoaderInitializationContext(serviceProvider));
		}

		[TearDown]
		public void TearDown()
		{
			this.ApplicationLoader?.Shutdown();
			this.ApplicationLoader = null;
		}
	}
}
