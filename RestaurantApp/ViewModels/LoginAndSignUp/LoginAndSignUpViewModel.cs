using Acr.UserDialogs;
using BaseMvvmToolkit.Commands;
using BaseMvvmToolkit.Services;
using BaseMvvmToolkit.ViewModels;
using RestaurantApp.Constants;
using RestaurantApp.Helpers;
using RestaurantApp.Models.Menu;
using RestaurantApp.Models.Offer;
using RestaurantApp.Models.User;
using RestaurantApp.Services.SqliteServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace RestaurantApp.ViewModels.LoginAndSignUp
{
    public class LoginAndSignUpViewModel : BaseViewModel
    {
        public UserModel User { get; set; }
        public IAsyncCommand LogInCommand { get; set; }
        public IAsyncCommand SignUpCommand { get; set; }
        public LoginAndSignUpViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Login";
            User = new UserModel();
            LogInCommand = new AsyncCommand(LogInCommandExecute);
            SignUpCommand = new AsyncCommand(SignUpCommandExecute);
        }

        private async Task SignUpCommandExecute()
        {
            if (!UserRegestrationValidation())
                return;
            try
            {
                UserDialogs.Instance.ShowLoading();
                await SqliteServices.InsertAsync(User);
                UserDialogs.Instance.Toast(new ToastConfig("Log in Now")
                { Position = ToastPosition.Top, Duration = TimeSpan.FromSeconds(2) });
                NavigationService.SetMainViewModel<LoginAndSignUpViewModel>();
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
        private bool UserRegestrationValidation()
        {
            if (string.IsNullOrWhiteSpace(User.UserName) ||
                string.IsNullOrWhiteSpace(User.Email) ||
                string.IsNullOrWhiteSpace(User.Password))
            {
                UserDialogs.Instance.Toast(new ToastConfig("Missing Data!")
                { Position = ToastPosition.Top, Duration = TimeSpan.FromSeconds(3) });
                return false;
            }
            if (!Regex.IsMatch(User.Email, AppConstants.EmailValidationRule))
            {
                UserDialogs.Instance.Toast(new ToastConfig("Invalid Email format")
                { Position = ToastPosition.Top, Duration = TimeSpan.FromSeconds(3) });
                return false;
            }

            return true;
        }
        private async Task LogInCommandExecute()
        {
            if (!UserLoginValidation())
                return;
            try
            {
                UserDialogs.Instance.ShowLoading();
                var userlist = await SqliteServices.GetListAsync<UserModel>();
                var user = userlist.FirstOrDefault(x =>
                x.UserName.ToLower() == User.UserName.ToLower() &&
                x.Password.ToLower() == User.Password.ToLower());
                if (user != null)
                {
                    GeneralSettings.IsUserloggedin = true;
                    GeneralSettings.ActiveUser = user;
                    NavigationService.SetMainViewModel<TabbedHomeViewModel>();
                }
                else
                {
                    UserDialogs.Instance.Toast(new ToastConfig("Inavlid Username or password!")
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
        private bool UserLoginValidation()
        {
            if (string.IsNullOrWhiteSpace(User.UserName) ||
                string.IsNullOrWhiteSpace(User.Password))
            {
                UserDialogs.Instance.Toast(new ToastConfig("Missing Data!")
                { Position = ToastPosition.Top, Duration = TimeSpan.FromSeconds(3) });
                return false;
            }
            if (!Regex.IsMatch(User.UserName, AppConstants.UserNameLenghtRule))
            {
                UserDialogs.Instance.Toast(new ToastConfig("Invalid Username Length")
                { Position = ToastPosition.Top, Duration = TimeSpan.FromSeconds(3) });
                return false;
            }

            return true;
        }

        private async Task InitializeLocalDatabase()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Loading data");
                #region DEMO Data Added To SQLite
                var OfferList = new List<OfferModel>()
                {
                    new OfferModel { ID ="1", Image="burger" },
                    new OfferModel { ID ="2", Image="burger1" },
                    new OfferModel { ID ="3", Image="burger2" },
                    new OfferModel { ID ="4", Image="burger3" }
                };
                var Categorylist = new List<CategoryModel>
                {
                    new CategoryModel{ID="1",Name="PROMOTIONS",Image ="burger"},
                    new CategoryModel{ID="2",Name="MEALS",Image ="burger1"},
                    new CategoryModel{ID="3",Name="SANDWICHES",Image ="burger3"},
                    new CategoryModel{ID="4",Name="SIDES",Image ="fries"},
                    new CategoryModel{ID="5",Name="DESSERTS",Image ="icecream1"},
                    new CategoryModel{ID="6",Name="OFFERS",Image ="logo"},
                };
                var ItemsList = new List<ItemModel>
                {
                    new ItemModel{CategoryID = "1", ID="1", Image="burger1",Name="BIG MAC",Price=35.50,Title="BIG MAC SANDWICH",},
                    new ItemModel{CategoryID = "1", ID="2", Image="burger1",Name="BIG MAC",Price=35.50,Title="BIG MAC SANDWICH",},
                    new ItemModel{CategoryID = "1", ID="3", Image="burger1",Name="BIG MAC",Price=35.50,Title="BIG MAC SANDWICH",},
                    new ItemModel{CategoryID = "1", ID="4", Image="burger1",Name="BIG MAC",Price=35.50,Title="BIG MAC SANDWICH",},
                    new ItemModel{CategoryID = "1", ID="5", Image="burger1",Name="BIG MAC",Price=35.50,Title="BIG MAC SANDWICH",},
                    new ItemModel{CategoryID = "1", ID="6", Image="burger1",Name="BIG MAC",Price=35.50,Title="BIG MAC SANDWICH",},

                    new ItemModel{CategoryID = "2", ID="7", Image="burger2",Name="Big Tasty",Price=75.50,Title="Big Tasty SANDWICH",},
                    new ItemModel{CategoryID = "2", ID="8", Image="burger2",Name="Big Tasty",Price=75.50,Title="Big Tasty SANDWICH",},
                    new ItemModel{CategoryID = "2", ID="9", Image="burger2",Name="Big Tasty",Price=75.50,Title="Big Tasty SANDWICH",},
                    new ItemModel{CategoryID = "2", ID="10", Image="burger2",Name="Big Tasty",Price=75.50,Title="Big Tasty SANDWICH",},
                    new ItemModel{CategoryID = "2", ID="11", Image="burger2",Name="Big Tasty",Price=75.50,Title="Big Tasty SANDWICH",},
                    new ItemModel{CategoryID = "2", ID="12", Image="burger2",Name="Big Tasty",Price=75.50,Title="Big Tasty SANDWICH",},

                    new ItemModel{CategoryID = "3", ID="13", Image="burger3",Name="Mega Mac",Price=30.50,Title="Mega Mac SANDWICH",},
                    new ItemModel{CategoryID = "3", ID="14", Image="burger3",Name="Mega Mac",Price=30.50,Title="Mega Mac SANDWICH",},
                    new ItemModel{CategoryID = "3", ID="15", Image="burger3",Name="Mega Mac",Price=30.50,Title="Mega Mac SANDWICH",},
                    new ItemModel{CategoryID = "3", ID="16", Image="burger3",Name="Mega Mac",Price=30.50,Title="Mega Mac SANDWICH",},
                    new ItemModel{CategoryID = "3", ID="17", Image="burger3",Name="Mega Mac",Price=30.50,Title="Mega Mac SANDWICH",},
                    new ItemModel{CategoryID = "3", ID="18", Image="burger3",Name="Mega Mac",Price=30.50,Title="Mega Mac SANDWICH",},

                    new ItemModel{CategoryID = "4", ID="19", Image="fries",Name="Fries",Price=15.00,Title="Fries SANDWICH",},
                    new ItemModel{CategoryID = "4", ID="20", Image="fries",Name="Fries",Price=15.00,Title="Fries SANDWICH",},
                    new ItemModel{CategoryID = "4", ID="21", Image="fries",Name="Fries",Price=15.00,Title="Fries SANDWICH",},
                    new ItemModel{CategoryID = "4", ID="22", Image="fries",Name="Fries",Price=15.00,Title="Fries SANDWICH",},
                    new ItemModel{CategoryID = "4", ID="23", Image="fries",Name="Fries",Price=15.00,Title="Fries SANDWICH",},
                    new ItemModel{CategoryID = "4", ID="24", Image="fries",Name="Fries",Price=15.00,Title="Fries SANDWICH",},

                    new ItemModel{CategoryID = "5", ID="25", Image="icecream",Name="Sunday",Price=30.50,Title="Sunday ",},
                    new ItemModel{CategoryID = "5", ID="26", Image="icecream1",Name="Sunday",Price=30.50,Title="Sunday ",},
                    new ItemModel{CategoryID = "5", ID="27", Image="icecream",Name="Sunday",Price=30.50,Title="Sunday ",},
                    new ItemModel{CategoryID = "5", ID="28", Image="icecream1",Name="Sunday",Price=30.50,Title="Sunday ",},
                    new ItemModel{CategoryID = "5", ID="29", Image="icecream",Name="Sunday",Price=30.50,Title="Sunday ",},
                    new ItemModel{CategoryID = "5", ID="30", Image="icecream1",Name="Sunday",Price=30.50,Title="Sunday ",},

                    new ItemModel{CategoryID = "6", ID="31", Image="burger1",Name="New Item",Price=30.50,Title="New Item ",},
                    new ItemModel{CategoryID = "6", ID="32", Image="icecream1",Name="New Item",Price=30.50,Title="New Item ",},
                    new ItemModel{CategoryID = "6", ID="33", Image="fries",Name="New Item",Price=30.50,Title="New Item ",},
                    new ItemModel{CategoryID = "6", ID="34", Image="burger3",Name="New Item",Price=30.50,Title="New Item ",},
                    new ItemModel{CategoryID = "6", ID="35", Image="icecream",Name="New Item",Price=30.50,Title="New Item ",},
                    new ItemModel{CategoryID = "6", ID="36", Image="logo",Name="New Item",Price=30.50,Title="New Item ",},
                };
                #endregion
                foreach (var item in OfferList)
                {
                    await SqliteServices.InsertAsync<OfferModel>(item);
                }
                foreach (var item in Categorylist)
                {
                    await SqliteServices.InsertAsync<CategoryModel>(item);
                }
                foreach (var item in ItemsList)
                {
                    await SqliteServices.InsertAsync<ItemModel>(item);
                }
                GeneralSettings.IsLocalDataCached = true;

            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Can't load database, try again");
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
                if (GeneralSettings.IsLocalDataCached)
                    return;
                await InitializeLocalDatabase();
            });
        }
    }
}
