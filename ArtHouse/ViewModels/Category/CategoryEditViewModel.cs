namespace ArtHouse.ViewModels.Category
{
    public class CategoryEditViewModel : CategoryCreateViewModel
    {
        public int Id { get; set; }

        public string? ExistingImageUrl { get; set; }
    }
}