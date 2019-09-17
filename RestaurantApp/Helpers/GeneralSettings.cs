using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using RestaurantApp.Models.User;

namespace RestaurantApp.Helpers
{
    public class GeneralSettings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public static bool IsUserloggedin
        {
            get => AppSettings.GetValueOrDefault(nameof(IsUserloggedin), false);
            set => AppSettings.AddOrUpdateValue(nameof(IsUserloggedin), value);
        }

        private static string _activeUser
        {
            get => AppSettings.GetValueOrDefault(nameof(_activeUser), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(_activeUser), value);
        }

        public static bool IsLocalDataCached
        {
            get => AppSettings.GetValueOrDefault(nameof(IsLocalDataCached), false);
            set => AppSettings.AddOrUpdateValue(nameof(IsLocalDataCached), value);
        }

        public static UserModel ActiveUser
        {
            get => JsonConvert.DeserializeObject<UserModel>(_activeUser);
            set => _activeUser = JsonConvert.SerializeObject(value);
        }
        public static void ClearallData()
        {
            AppSettings.Clear();
        }
    }
}
