using ArtHouse.Data;
using ArtHouse.Models;
using ArtHouse.Models.Enums;
using ArtHouse.Services.Interfaces;
using ArtHouse.ViewModels.Orders;
using Microsoft.EntityFrameworkCore;

namespace ArtHouse.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly AppDbContext _context;


        public CheckoutService(AppDbContext context)
        {
            _context = context;
        }

        #region PlaceOrderAsync
        public async Task<Order?> PlaceOrderAsync(string userId)
        {
            var cartItems = await _context.CartItems
                .Include(ci => ci.Product)
                .Include(ci => ci.Cart)
                .Where(ci => ci.Cart.UserId == userId)
                .ToListAsync();


            if (!cartItems.Any())
            {
                return null;
            }


            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalPrice = cartItems.Sum(ci => ci.Product.Price * ci.Quantity),
                Status = OrderStatus.Pending
            };


            _context.Orders.Add(order);

            await _context.SaveChangesAsync();


            foreach (var item in cartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Product.Price
                };


                _context.OrderItems.Add(orderItem);
            }


            _context.CartItems.RemoveRange(cartItems);


            await _context.SaveChangesAsync();


            return order;
        }
        #endregion

        #region GetCheckoutAsync

        public async Task<CheckoutViewModel?> GetCheckoutAsync(string userId)
        {
            var cartItems = await _context.CartItems
                .Include(ci => ci.Product)
                .Include(ci => ci.Cart)
                .Where(ci => ci.Cart.UserId == userId)
                .ToListAsync();


            if (!cartItems.Any())
            {
                return null;
            }


            return new CheckoutViewModel
            {
                Items = cartItems.Select(ci => new CheckoutItemViewModel
                {
                    ProductId = ci.ProductId,
                    Title = ci.Product.Title,
                    ImageUrl = ci.Product.ImageUrl,
                    Price = ci.Product.Price,
                    Quantity = ci.Quantity

                }).ToList(),

                TotalItems = cartItems.Sum(ci => ci.Quantity),

                TotalPrice = cartItems.Sum(ci => ci.Product.Price * ci.Quantity)
            };
        }

        #endregion



    }
}