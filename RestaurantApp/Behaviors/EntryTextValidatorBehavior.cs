using Acr.UserDialogs;
using RestaurantApp.Constants;
using RestaurantApp.Controls;
using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace RestaurantApp.Behaviors
{
    public class EntryTextValidatorBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnEntryTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnEntryTextChanged;
        }
        int textlength = 0;
        void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            textlength++;
            var entry = (ImageEntry)sender;
            if (string.IsNullOrEmpty(entry.Text))
            {
                return;
            }
            var entryText = entry.Text.ToCharArray();
            var lastchar = entryText[entryText.Length - 1];
            // if Entry text Contains any number will not right it 


            if (!Regex.IsMatch(lastchar.ToString(), AppConstants.UserNameContentRule))
            {
                // remove last char
                entry.Text = entry.Text.Remove(entry.Text.Length - 1);
                UserDialogs.Instance.Toast(new ToastConfig("Special Char not allowed")
                { Position = ToastPosition.Top, Duration = TimeSpan.FromSeconds(3) });
            }
            if (entry.Text.Length > 15)
            {
                entry.Text = entry.Text.Remove(entry.Text.Length - 1);
                UserDialogs.Instance.Toast(new ToastConfig("Length must be less than 15")
                { Position = ToastPosition.Top, Duration = TimeSpan.FromSeconds(3) });
            }
            else if (entry.Text.Length >= 3)
            {
                entry.Image = "iconright";
                entry.TextColor = Color.Yellow;
            }
            else if (entry.Text.Length < 3)
            {
                entry.Image = "";
                entry.TextColor = Color.Black;
            }
        }
    }
}
