using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RestaurantApp.Pages.LoginAndSignUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginAndSignUpPage
    {
        public LoginAndSignUpPage()
        {
            InitializeComponent();
        }

        private void LabelHeaderRecognizer_Tapped(object sender, EventArgs e)
        {
            var lbl = sender as Label;
            var lbltxt = lbl.Text;
            switch (lbltxt)
            {
                case "LOGIN":
                    loginbutton.IsVisible = true;
                    loginbutton.IsVisible = true;
                    LoginUnderLine.IsVisible = true;
                    email.IsVisible = false;
                    registerbutton.IsVisible = false;
                    RegisterUnderLine.IsVisible = false;
                    break;
                case "REGISTER":
                    loginbutton.IsVisible = false;
                    loginbutton.IsVisible = false;
                    LoginUnderLine.IsVisible = false;
                    email.IsVisible = true;
                    registerbutton.IsVisible = true;
                    RegisterUnderLine.IsVisible = true;
                    break;
            }
        }
    }
}