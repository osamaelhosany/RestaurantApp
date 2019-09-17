using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RestaurantApp.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ToolBar
    {
        public static BindableProperty CloseButtonCommandProperty = BindableProperty.Create(nameof(CloseButtonCommand), typeof(ICommand), typeof(ToolBar));
        public static BindableProperty ToolbarClickCommandProperty = BindableProperty.Create(nameof(ToolbarClickCommand), typeof(ICommand), typeof(ToolBar));
        public static readonly BindableProperty ToolbarTitleProperty = BindableProperty.Create(nameof(ToolbarTitle), typeof(string), typeof(ToolBar), string.Empty);
        public static readonly BindableProperty ToolbarIconProperty = BindableProperty.Create(nameof(ToolbarIcon), typeof(string), typeof(ToolBar), string.Empty);
        public static readonly BindableProperty CloseIconProperty = BindableProperty.Create(nameof(CloseIcon), typeof(string), typeof(ToolBar), string.Empty);
        public static readonly BindableProperty ToolbarRightTextProperty = BindableProperty.Create(nameof(ToolbarRightText), typeof(string), typeof(ToolBar), string.Empty);
        public ToolBar()
        {
            InitializeComponent();
        }

        public ICommand CloseButtonCommand
        {
            get { return (ICommand)GetValue(CloseButtonCommandProperty); }
            set { SetValue(CloseButtonCommandProperty, value); }
        }
        public ICommand ToolbarClickCommand
        {
            get { return (ICommand)GetValue(ToolbarClickCommandProperty); }
            set { SetValue(ToolbarClickCommandProperty, value); }
        }

        public string ToolbarTitle
        {
            get { return (string)GetValue(ToolbarTitleProperty); }
            set { SetValue(ToolbarTitleProperty, value); }
        }

        public string ToolbarIcon
        {
            get { return (string)GetValue(ToolbarIconProperty); }
            set { SetValue(ToolbarIconProperty, value); }
        }

        public string CloseIcon
        {
            get { return (string)GetValue(CloseIconProperty); }
            set { SetValue(CloseIconProperty, value); }
        }
        public string ToolbarRightText
        {
            get { return (string)GetValue(ToolbarRightTextProperty); }
            set { SetValue(ToolbarRightTextProperty, value); }
        }
    }
}