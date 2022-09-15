namespace MauiApp1
{
	using Fluxera.Extensions.Hosting;

	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			return MauiApplicationHost.BuildApplication<MauiApp1Host>();
		}
	}
}
