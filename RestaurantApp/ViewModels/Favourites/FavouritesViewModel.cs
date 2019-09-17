using BaseMvvmToolkit.Services;
using BaseMvvmToolkit.ViewModels;

namespace RestaurantApp.ViewModels.Favourites
{
    public class FavouritesViewModel : BaseViewModel
    {
        public FavouritesViewModel(INavigationService navigationService) : base(navigationService)
        {
            Icon = "staricon";
            Title = "Favourites";
        }
    }
}
