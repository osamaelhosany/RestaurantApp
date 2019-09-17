namespace RestaurantApp.Models.User
{
    public class UserModel : BaseModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
