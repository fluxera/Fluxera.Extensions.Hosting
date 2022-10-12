namespace MauiApp1
{
	using Fluxera.Extensions.Hosting;
	using Fluxera.Extensions.Hosting.Modules;

	public class MauiApp1Module : ConfigureApplicationModule
	{
		/// <inheritdoc />
		public override void ConfigureServices(IServiceConfigurationContext context)
		{
			context.Services.AddSingleton(Connectivity.Current);
			context.Services.AddSingleton<MainPage>();
		}

		/// <inheritdoc />
		public override void OnApplicationShutdown(IApplicationShutdownContext context)
		{
			base.OnApplicationShutdown(context);
		}
	}
}
