namespace RestaurantApp.Models.Menu
{
    public class CategoryModel : BaseModel
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public bool IsSelected { get; set; }
    }
}