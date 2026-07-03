using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArtHouse.ViewModels.Products
{
    public class ProductCreateViewModel
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public string? ImageUrl { get; set; }

        public IFormFile? Image { get; set; }

        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }
        
        public List<SelectListItem> Categories { get; set; } = new();
    }
}