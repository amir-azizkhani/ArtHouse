namespace ArtHouse.ViewModels.Cart
{
    public class CartItemViewModel
    {
        public int CartItemId { get; set; }

        public int ProductId { get; set; }

        public string ProductTitle { get; set; } = string.Empty;

        public string? ProductImageUrl { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal SubTotal => UnitPrice * Quantity;
    }
}