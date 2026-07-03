namespace ArtHouse.ViewModels
{
    public class ProductItemViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Stock { get; set; }
    }
}