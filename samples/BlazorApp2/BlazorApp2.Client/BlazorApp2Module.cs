namespace BlazorApp2.Client
{
	using System;
	using System.Net.Http;
	using Fluxera.Extensions.DependencyInjection;
	using Fluxera.Extensions.Hosting;
	using Fluxera.Extensions.Hosting.Modules;
	using JetBrains.Annotations;
	using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
	using Microsoft.Extensions.DependencyInjection;

	[PublicAPI]
	[UsedImplicitly]
	internal sealed class BlazorApp2Module : ConfigureServicesModule
	{
		/// <inheritdoc />
		public override void ConfigureServices(IServiceConfigurationContext context)
		{
			IWebAssemblyHostEnvironment hostEnvironment = context.Services.GetObject<IWebAssemblyHostEnvironment>();
			context.Services.AddScoped(_ => new HttpClient
			{
				BaseAddress = new Uri(hostEnvironment.BaseAddress)
			});
		}
	}
}
