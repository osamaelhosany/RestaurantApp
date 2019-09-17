using Acr.UserDialogs;
using BaseMvvmToolkit.Services;
using BaseMvvmToolkit.ViewModels;
using RestaurantApp.Models.Menu;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels.Menu
{
    public class ItemDetailsViewModel : BaseViewModel
    {
        public ItemModel CurrentItem { get; set; }
        public ICommand NavigationButtonCommand { get; }
        public ICommand AddToFavoriteCommand { get; }
        public ICommand PlaceOrderCommand { get; }
        public ItemDetailsViewModel(INavigationService navigationService) : base(navigationService)
        {
            CurrentItem = new ItemModel();
            NavigationButtonCommand = new Command(NavigationButtonCommandExecute);
            AddToFavoriteCommand = new Command(AddToFavoriteCommandExecute);
            PlaceOrderCommand = new Command(PlaceOrderCommandExecute);
        }

        private void PlaceOrderCommandExecute(object obj)
        {
            UserDialogs.Instance.Toast("Not implemented yet!");
        }

        private void AddToFavoriteCommandExecute(object obj)
        {
            UserDialogs.Instance.Toast("Not implemented yet!");
        }

        private void NavigationButtonCommandExecute(object obj)
        {
            NavigationService.PopAsync();
        }

        public override Task Init(object obj)
        {
            if (obj is ItemModel _item)
                CurrentItem = _item;
            else
                UserDialogs.Instance.Toast("Something went wrong,please go back");

            return base.Init(obj);
        }
    }
}
