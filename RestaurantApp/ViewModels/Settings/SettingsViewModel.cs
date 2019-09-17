using BaseMvvmToolkit.Services;
using BaseMvvmToolkit.ViewModels;
using RestaurantApp.Helpers;
using RestaurantApp.ViewModels.LoginAndSignUp;
using System.Windows.Input;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels.Settings
{
    public class SettingsViewModel : BaseViewModel
    {
        public ICommand LogOutCommand { get; set; }
        public SettingsViewModel(INavigationService navigationService) : base(navigationService)
        {
            Icon = "menuicon";
            Title = "More";
            LogOutCommand = new Command(() =>
            {
                GeneralSettings.IsUserloggedin = false;
                NavigationService.SetMainViewModel<LoginAndSignUpViewModel>();
            });
        }
    }
}