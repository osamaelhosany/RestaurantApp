using BaseMvvmToolkit.Services;
using BaseMvvmToolkit.ViewModels;
using RestaurantApp.ViewModels.Favourites;
using RestaurantApp.ViewModels.Home;
using RestaurantApp.ViewModels.Menu;
using RestaurantApp.ViewModels.Order;
using RestaurantApp.ViewModels.Settings;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels
{
    public class TabbedHomeViewModel : TabbedViewModel<HomeViewModel, MenuViewModel,
                                                       FavouritesViewModel, TrackOrderViewModel,
                                                       SettingsViewModel>
    {
        public TabbedHomeViewModel(INavigationService navigationService) : base(navigationService)
        {
            BarBackgroundColor = Color.White;
            BarItemColor = Color.DarkGray;
            BarSelectedItemColor = Color.Red;
        }
    }
}
