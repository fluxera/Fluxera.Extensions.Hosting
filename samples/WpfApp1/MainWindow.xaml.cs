namespace WpfApp1
{
	using System.Windows;
	using Fluxera.Extensions.Hosting;

	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, IMainWindow
	{
		public MainWindow()
		{
			this.InitializeComponent();
		}
	}
}
