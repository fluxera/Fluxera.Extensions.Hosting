namespace XamarinApp1
{
	using Fluxera.Extensions.Hosting;
	using Fluxera.Extensions.Hosting.Modules;
	using Microsoft.Extensions.DependencyInjection;

	public class XamarinApp1Module : ConfigureServicesModule
	{
		/// <inheritdoc />
		public override void ConfigureServices(IServiceConfigurationContext context)
		{
			context.Services.AddSingleton<MainPage>();
		}
	}
}
