namespace ArtHouse.ViewModels
{
    public class ProductListViewModel
    {
        public List<ProductItemViewModel> Products { get; set; } = new();

        public string? Search { get; set; }

        public int TotalCount { get; set; }
    }
}