using ArtHouse.ViewModels.Products;

namespace ArtHouse.ViewModels.Orders
{
    public class CheckoutViewModel
    {
        public List<CheckoutItemViewModel> Items { get; set; } = new();

        public decimal TotalPrice { get; set; }

        public int TotalItems { get; set; }
    }
}