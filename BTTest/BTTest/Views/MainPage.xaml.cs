using BTTest.ViewModels;
using Xamarin.Forms;

namespace BTTest
{
    public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
            BindingContext = new MainPageViewModel();
		}
	}
}
