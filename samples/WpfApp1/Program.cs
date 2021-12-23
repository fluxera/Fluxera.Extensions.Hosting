namespace WpfApp1
{
	using System.Threading.Tasks;
	using Fluxera.Extensions.Hosting;

	public static class Program
	{
		public static async Task Main(string[] args)
		{
			//Application application = new Application
			//{
			//	StartupUri = new Uri("MainWindow.xaml", UriKind.Relative)
			//};
			//application.Run();

			await ApplicationHost.RunAsync<WpfApp1Host>(args);
		}
	}
}
