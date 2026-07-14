namespace ArtHouse.ViewModels.Category
{
    public class CategoryCreateViewModel
    {
        public string Name { get; set; } = string.Empty;

        public IFormFile? Image { get; set; }

        public string? ImageUrl { get; set; }
    }
}