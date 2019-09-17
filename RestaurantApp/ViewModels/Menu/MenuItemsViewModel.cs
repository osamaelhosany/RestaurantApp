using Acr.UserDialogs;
using BaseMvvmToolkit.Services;
using BaseMvvmToolkit.ViewModels;
using RestaurantApp.Models.Menu;
using RestaurantApp.Services.SqliteServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels.Menu
{
    public class MenuItemsViewModel : BaseViewModel
    {
        public ObservableCollection<CategoryModel> CategoryList { get; set; }
        public ObservableCollection<ItemModel> ItemsList { get; set; }
        public CategoryModel SelectedCategory { get; set; }
        public ICommand OnSelectedItemCommand { get; }
        public ICommand OnHeaderSelectedCommand { get; }
        public ICommand NavigationButtonCommand { get; }
        public MenuItemsViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Menu";
            SelectedCategory = new CategoryModel();
            CategoryList = new ObservableCollection<CategoryModel>();
            ItemsList = new ObservableCollection<ItemModel>();
            OnSelectedItemCommand = new Command(OnSelectedItemCommandExecute);
            OnHeaderSelectedCommand = new Command(OnHeaderSelectedCommandExecute);
            NavigationButtonCommand = new Command(NavigationButtonCommandExecute);
        }

        private void NavigationButtonCommandExecute(object obj)
        {
            NavigationService.PopAsync();
        }

        private void OnHeaderSelectedCommandExecute(object obj)
        {
            if (obj is CategoryModel headerselected)
            {
                CategoryList.Select(x => { x.IsSelected = false; return x; }).ToList();
                headerselected.IsSelected = true;
                SelectedCategory = headerselected;
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await GetDataAsync();
                });
            }
        }

        private void OnSelectedItemCommandExecute(object obj)
        {
            NavigationService.NavigateToAsync<ItemDetailsViewModel>(obj);
        }
        public async Task GetDataAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                var list = await SqliteServices.GetListAsync<ItemModel>();
                if (list != null && list.Any())
                {
                    var index = CategoryList.IndexOf(SelectedCategory);
                    MessagingCenter.Send<App, int>((App)Xamarin.Forms.Application.Current, "MessageIndex", index);
                    ItemsList = new ObservableCollection<ItemModel>(list.Where(x => x.CategoryID == SelectedCategory.ID));
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
                if (ItemsList.Any()) return;
                await GetDataAsync();
            });
        }
        public override Task Init(object obj)
        {
            if (obj is List<CategoryModel> list)
            {
                CategoryList = new ObservableCollection<CategoryModel>(list);
                SelectedCategory = CategoryList.FirstOrDefault(x => x.IsSelected == true);
            }
            return base.Init(obj);
        }
    }
}
