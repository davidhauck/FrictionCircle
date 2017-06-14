using FrictionCircle.ViewModels;
using Xamarin.Forms;

namespace FrictionCircle.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }
    }
}
