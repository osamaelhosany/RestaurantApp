
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RestaurantApp.Pages.Menu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuItemsPage
    {
        public MenuItemsPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<App, int>((App)Xamarin.Forms.Application.Current, "MessageIndex", (sender, value) =>
            {
                Headerlist.GetChildWithIndex(value);
            });
        }
    }
}