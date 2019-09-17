using Autofac;
using BaseMvvmToolkit.Extensions;
using BaseMvvmToolkit.Services;
using RestaurantApp.Helpers;
using RestaurantApp.ViewModels;
using RestaurantApp.ViewModels.LoginAndSignUp;
using System.Reflection;
using Xamarin.Forms;

namespace RestaurantApp
{
    public static class AppContainer
    {
        public static IContainer Container { get; set; }
    }
    public partial class App : Application
    {
        public IContainer Container { get; private set; }
        public App()
        {
            InitializeComponent();
            SetupDependencyInjection();
            SetStartPage();
        }
        private void SetupDependencyInjection()
        {
            if (Container != null)
            {
                return;
            }
            var builder = new ContainerBuilder();
            builder.RegisterMvvmComponents(typeof(App).GetTypeInfo().Assembly);
            builder.RegisterType<NavigationService>().AsImplementedInterfaces().SingleInstance();
            Container = builder.Build();
            AppContainer.Container = Container;
        }

        private void SetStartPage()
        {
            var navigationService = Container.Resolve<INavigationService>();
            if (GeneralSettings.IsUserloggedin)
            {
                navigationService.SetMainViewModel<TabbedHomeViewModel>();
            }
            else
            {
                navigationService.SetMainViewModel<LoginAndSignUpViewModel>();
            }
        }
    }
}
