namespace MauiApp1
{
	using Fluxera.Extensions.Hosting;
	using Fluxera.Extensions.Hosting.Modules;

	public class MauiApp1Module : ConfigureServicesModule
	{
		/// <inheritdoc />
		public override void ConfigureServices(IServiceConfigurationContext context)
		{
			context.Services.AddSingleton<IConnectivity>(Connectivity.Current);
			context.Services.AddSingleton<MainPage>();
		}
	}
}
