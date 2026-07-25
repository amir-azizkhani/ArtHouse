namespace ArtHouse.ViewModels.Orders
{
    public class OrderListViewModel
    {
        public List<OrderItemListViewModel> Orders { get; set; } = new();
    }
}