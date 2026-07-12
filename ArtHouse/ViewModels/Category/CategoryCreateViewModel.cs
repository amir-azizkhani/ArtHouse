namespace ArtHouse.ViewModels.Category
{
    public class CategoryCreateViewModel
    {
        public string Name { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public IFormFile? Image { get; set; }
    }
}


