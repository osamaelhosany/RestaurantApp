using BaseMvvmToolkit.Services;
using BaseMvvmToolkit.ViewModels;

namespace RestaurantApp.ViewModels.Order
{
    public class TrackOrderViewModel : BaseViewModel
    {
        public TrackOrderViewModel(INavigationService navigationService) : base(navigationService)
        {
            Icon = "truckicon";
            Title = "Orders";
        }
    }
}
