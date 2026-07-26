using ArtHouse.ViewModels.Cart;

namespace ArtHouse.Services.Interfaces
{
    public interface ICartService
    {
        Task AddToCartAsync(string userId, int productId);

        Task<bool> IncreaseQuantityAsync(int cartItemId);

        Task<bool> DecreaseQuantityAsync(int cartItemId);

        Task<bool> RemoveAsync(int cartItemId);

        Task<CartViewModel> GetCartAsync(string userId);
    }
}