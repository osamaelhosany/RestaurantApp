using Acr.UserDialogs;
using RestaurantApp.Constants;
using RestaurantApp.Controls;
using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace RestaurantApp.Behaviors
{
    public class EntryEmailValidatorBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnEntryTextChanged;
            bindable.Unfocused += Bindable_Completed;
        }


        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnEntryTextChanged;
            bindable.Unfocused -= Bindable_Completed;
        }
        private void Bindable_Completed(object sender, EventArgs e)
        {
            var entry = (ImageEntry)sender;
            if (string.IsNullOrEmpty(entry.Text))
            {
                return;
            }

            if (!Regex.IsMatch(entry.Text.ToString(), AppConstants.EmailValidationRule))
            {
                // remove last char
                entry.TextColor = Color.Black;
                entry.Image = "";
                UserDialogs.Instance.Toast(new ToastConfig("Invalid Email format")
                { Position = ToastPosition.Top, Duration = TimeSpan.FromSeconds(3) });
            }
            else
            {
                entry.TextColor = Color.Yellow;
                entry.Image = "iconright";
            }
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
