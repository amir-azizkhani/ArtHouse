namespace ArtHouse.ViewModels.Orders
{
    public class OrderDetailsViewModel
    {
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalPrice { get; set; }

        public List<OrderDetailsItemViewModel> Items { get; set; } = new();
    }
}