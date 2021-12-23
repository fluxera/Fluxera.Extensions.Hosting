using Xamarin.Forms;

namespace XamarinApp1
{
	using Microsoft.Extensions.Logging;

	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			this.InitializeComponent();
		}

		public MainPage(ILogger<MainPage> logger) : this()
		{
			logger.LogInformation("Hello from MainPage!");
		}
	}
}
