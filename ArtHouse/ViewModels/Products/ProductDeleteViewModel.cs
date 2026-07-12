using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArtHouse.ViewModels.Products
{
    public class ProductDeleteViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public string? CategoryName { get; set; }


    }
}
