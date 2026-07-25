using ArtHouse.Models.Enums;

namespace ArtHouse.ViewModels.Orders

{
    public class OrderItemListViewModel
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalPrice { get; set; }

        public int TotalItems { get; set; }

        public string UserName { get; set; } = string.Empty;

        public OrderStatus Status { get; set; }
    }
}