using Acr.UserDialogs;
using BaseMvvmToolkit.Services;
using BaseMvvmToolkit.ViewModels;
using RestaurantApp.Models.Menu;
using RestaurantApp.Services.SqliteServices;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels.Menu
{
    public class MenuViewModel : BaseViewModel
    {
        public ObservableCollection<CategoryModel> CategoryList { get; set; }
        public ICommand OnSelectedCategoryCommand { get; }
        public MenuViewModel(INavigationService navigationService) : base(navigationService)
        {
            Icon = "bookicon";
            Title = "MENU";
            CategoryList = new ObservableCollection<CategoryModel>();
            OnSelectedCategoryCommand = new Command(OnSelectedCategoryCommandExecute);
        }

        private void OnSelectedCategoryCommandExecute(object obj)
        {
            if (obj is CategoryModel selecteditem)
            {
                CategoryList.Select(x => { x.IsSelected = false; return x; }).ToList();
                selecteditem.IsSelected = true;
                NavigationService.NavigateToAsync<MenuItemsViewModel>(CategoryList.ToList());
            }
        }
        public async Task GetDataAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                var list = await SqliteServices.GetListAsync<CategoryModel>();

                if (list != null && list.Any())
                {
                    CategoryList = new ObservableCollection<CategoryModel>(list);
                }
                else
                {
                    UserDialogs.Instance.Toast(new ToastConfig("Sorry, Can't Fetch Data")
                    { Position = ToastPosition.Top, Duration = TimeSpan.FromSeconds(3) });
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Something went wrong, try again");
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
                if (CategoryList.Any()) return;
                await GetDataAsync();
            });

        }
    }

}
