namespace ArtHouse.ViewModels.Orders
{
    public class OrderDetailsItemViewModel
    {
        public string Title { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice => Price * Quantity;
    }
}