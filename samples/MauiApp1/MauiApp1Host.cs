namespace MauiApp1
{
	using Fluxera.Extensions.Hosting;
	using Microsoft.Extensions.Logging;

	public class MauiApp1Host : MauiApplicationHost<MauiApp1Module, App>
	{
		/// <inheritdoc />
		protected override void ConfigureHostBuilder(MauiAppBuilder builder)
		{
			base.ConfigureHostBuilder(builder);

			builder.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
			builder.Logging.AddDebug();
#endif
		}
	}
}
