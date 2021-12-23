namespace Fluxera.Extensions.Hosting
{
	using System.Collections.Generic;
	using Fluxera.Extensions.Hosting.Modules;
	using JetBrains.Annotations;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;

	/// <summary>
	///     An abstract base class for web host applications.
	/// </summary>
	/// <typeparam name="TStartupModule"></typeparam>
	[PublicAPI]
	public abstract class WebApplicationHost<TStartupModule> : ApplicationHost<TStartupModule>
		where TStartupModule : class, IModule
	{
		private WebApplicationBuilder webApplicationBuilder = null!;

		/// <inheritdoc />
		protected override ApplicationLoaderBuilderFunc ApplicationLoaderBuilder => ApplicationLoaderBuilderFuncFactory.CreateApplication;

		/// <inheritdoc />
		protected override IEnumerable<string> HostConfigurationEnvironmentVariablesPrefixes
		{
			get
			{
				foreach(string prefix in base.HostConfigurationEnvironmentVariablesPrefixes)
				{
					yield return prefix;
				}

				yield return "ASPNETCORE_";
			}
		}

		/// <inheritdoc />
		protected override IHostBuilder CreateHostBuilder()
		{
			// Create the host builder and configure it.
			this.webApplicationBuilder = WebApplication.CreateBuilder(this.CommandLineArgs);
			return this.webApplicationBuilder.Host;
		}

		/// <inheritdoc />
		protected override IHost BuildHost()
		{
			// Build the host.
			return this.webApplicationBuilder.Build();
		}

		/// <inheritdoc />
		protected override void InitializeApplicationLoader(IHost host)
		{
			WebApplication webHost = (WebApplication)host;
			IApplicationLoader applicationLoader = host.Services.GetRequiredService<IApplicationLoader>();
			applicationLoader.Initialize(new WebApplicationLoaderInitializationContext(webHost));
		}
	}
}
