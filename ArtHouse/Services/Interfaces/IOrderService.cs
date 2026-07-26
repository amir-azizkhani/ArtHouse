using ArtHouse.Models;
using ArtHouse.Models.Enums;

namespace ArtHouse.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string userId);

        Task<bool> UpdateStatusAsync(int orderId, OrderStatus status);

        Task<List<Order>> GetAllOrdersAsync();

        Task<List<Order>> GetUserOrdersAsync(string userId);

        Task<Order?> GetOrderByIdAsync(int orderId);

        Task<Order?> GetOrderForSuccessAsync(int orderId);
    }
}