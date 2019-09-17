namespace RestaurantApp.Constants
{
    public static class AppConstants
    {
        public const string UserNameContentRule = @"^[a-zA-Z\u00c0-\u017e]$";
        public const string UserNameLenghtRule = @"^.{3,15}$";
        public const string EmailValidationRule = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
        public const string BaseEndPointOfflineMode = "https://offlinetester123.azurewebsites.net";
    }
}
