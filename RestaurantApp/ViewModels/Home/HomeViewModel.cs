using Acr.UserDialogs;
using BaseMvvmToolkit.Services;
using BaseMvvmToolkit.ViewModels;
using RestaurantApp.Models.Offer;
using RestaurantApp.Services.SqliteServices;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace RestaurantApp.ViewModels.Home
{
    public class HomeViewModel : BaseViewModel
    {
        public ObservableCollection<OfferModel> OfferList { get; set; }

        public int SelectedIndex { get; set; }
        public HomeViewModel(INavigationService navigationService) : base(navigationService)
        {
            Icon = "homeicon";
            Title = "Home";
            OfferList = new ObservableCollection<OfferModel>();
        }
        private async Task GetDataAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                var list = await SqliteServices.GetListAsync<OfferModel>();
                if (list != null && list.Any())
                {
                    OfferList = new ObservableCollection<OfferModel>(list);
                }
                else
                {
                    UserDialogs.Instance.Toast(new ToastConfig("Sorry, Can't Fetch Data")
                    { Position = ToastPosition.Top, Duration = TimeSpan.FromSeconds(3) });
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }
        public override void OnAppearing()
        {
            base.OnAppearing();
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (OfferList.Any()) return;
                await GetDataAsync();
            });
        }
    }
}
