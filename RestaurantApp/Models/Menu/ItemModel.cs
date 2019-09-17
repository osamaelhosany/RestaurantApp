namespace RestaurantApp.Models.Menu
{
    public class ItemModel : BaseModel
    {
        public string CategoryID { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public bool IsFavorite { get; set; }
        public double Price { get; set; }

    }
}
