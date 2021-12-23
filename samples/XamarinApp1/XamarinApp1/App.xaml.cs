namespace XamarinApp1
{
	using System.Threading.Tasks;
	using Fluxera.Extensions.Hosting;
	using Microsoft.Extensions.DependencyInjection;

	public partial class App : XamarinApplication
	{
		public App()
		{
			this.InitializeComponent();
		}

		protected override void OnStart()
		{
			Task.Run(async () => await this.Host.StartAsync());
			this.MainPage = this.Host.Services.GetRequiredService<MainPage>();
		}

		protected override void OnSleep()
		{
			Task.Run(async () => await this.Host.SleepAsync());
		}

		protected override void OnResume()
		{
			Task.Run(async () => await this.Host.ResumeAsync());
		}
	}
}
