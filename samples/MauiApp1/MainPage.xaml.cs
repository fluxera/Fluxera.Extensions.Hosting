namespace MauiApp1
{
	public partial class MainPage : ContentPage
	{
		private int count;

		public MainPage()
		{
			this.InitializeComponent();
		}

		private void OnCounterClicked(object sender, EventArgs e)
		{
			this.count++;

			this.CounterBtn.Text = this.count == 1 
				? $"Clicked {this.count} time" 
				: $"Clicked {this.count} times";

			SemanticScreenReader.Announce(this.CounterBtn.Text);
		}
	}
}
