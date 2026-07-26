using ArtHouse.Models;
using ArtHouse.ViewModels.Orders;

namespace ArtHouse.Services.Interfaces
{
    public interface ICheckoutService
    {
        Task<Order?> PlaceOrderAsync(string userId);
        Task<CheckoutViewModel?> GetCheckoutAsync(string userId);
    }
}