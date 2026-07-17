namespace ArtHouse.ViewModels.Products
{
    public class ProductListViewModel
    {
        public List<ProductItemViewModel> Products { get; set; } = new();

        public string? Search { get; set; }

        public int TotalCount { get; set; }

        public int? SelectedCategoryId { get; set; }

        public string? SelectedCategoryName { get; set; }
    }
}