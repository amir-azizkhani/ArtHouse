namespace ArtHouse.ViewModels.Cart
{
    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new();

        public decimal TotalPrice => Items.Sum(i => i.SubTotal);

        public int TotalItems => Items.Sum(i => i.Quantity);
    }
}