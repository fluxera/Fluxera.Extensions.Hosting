namespace MauiApp1
{
	using CommunityToolkit.Maui;
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

			// Register the MAUI community toolkit.
			builder.UseMauiCommunityToolkit();
			//builder.UseMauiCommunityToolkitMarkup();
			//builder.UseMauiCommunityToolkitMaps("key");
			//builder.UseMauiCommunityToolkitMediaElement();

#if DEBUG
			builder.Logging.AddDebug();
#endif
		}
	}
}
