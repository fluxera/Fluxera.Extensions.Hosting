namespace BlazorApp1
{
	using Fluxera.Extensions.Hosting;
	using Fluxera.Extensions.Hosting.Modules;
	using JetBrains.Annotations;

	[PublicAPI]
	[UsedImplicitly]
	internal sealed class BlazorApp1Module : ConfigureServicesModule
	{
		/// <inheritdoc />
		public override void ConfigureServices(IServiceConfigurationContext context)
		{
			//context.Services.AddScoped(serviceProvider => new HttpClient
			//{
			//	BaseAddress = new Uri(context.Environment.BaseAddress)
			//});
		}
	}
}
