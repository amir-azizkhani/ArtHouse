namespace ArtHouse.ViewModels.Products
{
    public class ProductEditViewModel : ProductCreateViewModel
    {
        public int Id { get; set; }

        public string? ExistingImageUrl { get; set; }
    }
}
