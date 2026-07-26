using ArtHouse.Data;
using ArtHouse.Models;
using ArtHouse.Services.Interfaces;
using ArtHouse.ViewModels.Cart;
using Microsoft.EntityFrameworkCore;

namespace ArtHouse.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;


        public CartService(AppDbContext context)
        {
            _context = context;
        }


        #region AddToCartAsync
        public async Task AddToCartAsync(string userId, int productId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);


            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId
                };


                _context.Carts.Add(cart);

                await _context.SaveChangesAsync();
            }



            var cartItem = cart.CartItems
                .FirstOrDefault(ci => ci.ProductId == productId);



            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = 1
                };


                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity++;
            }



            await _context.SaveChangesAsync();
        }
        #endregion

        #region IncreaseQuantityAsync
        public async Task<bool> IncreaseQuantityAsync(int cartItemId)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId);


            if (cartItem == null)
            {
                return false;
            }


            cartItem.Quantity++;


            await _context.SaveChangesAsync();

            return true;
        }
        #endregion

        #region DecreaseQuantityAsync
        public async Task<bool> DecreaseQuantityAsync(int cartItemId)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId);


            if (cartItem == null)
            {
                return false;
            }


            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
            }


            await _context.SaveChangesAsync();


            return true;
        }
        #endregion

        #region RemoveAsync
        public async Task<bool> RemoveAsync(int cartItemId)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId);


            if (cartItem == null)
            {
                return false;
            }


            _context.CartItems.Remove(cartItem);


            await _context.SaveChangesAsync();


            return true;
        }
        #endregion

        #region GetCartAsync
        public async Task<CartViewModel> GetCartAsync(string userId)
        {
            var viewModel = new CartViewModel();


            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);



            if (cart == null)
            {
                return viewModel;
            }



            foreach (var item in cart.CartItems)
            {
                viewModel.Items.Add(new CartItemViewModel
                {
                    CartItemId = item.Id,
                    ProductId = item.ProductId,
                    ProductTitle = item.Product.Title,
                    ProductImageUrl = item.Product.ImageUrl,
                    UnitPrice = item.Product.Price,
                    Quantity = item.Quantity
                });
            }


            return viewModel;
        }
        #endregion


    }
}