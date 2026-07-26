using ArtHouse.Data;
using ArtHouse.Models;
using ArtHouse.Models.Enums;
using ArtHouse.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ArtHouse.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;


        public OrderService(AppDbContext context)
        {
            _context = context;
        }


        #region CreateOrderAsync
        public async Task<Order?> CreateOrderAsync(string userId)
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
                TotalPrice = cartItems.Sum(ci => ci.Product.Price * ci.Quantity)
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

        #region UpdateStatusAsync


        public async Task<bool> UpdateStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
            {
                return false;
            }


            order.Status = status;

            await _context.SaveChangesAsync();


            return true;
        }


        #endregion

        #region GetAllOrdersAsync

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }


        #endregion

        #region GetUserOrdersAsync

        public async Task<List<Order>> GetUserOrdersAsync(string userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }


        #endregion

        #region GetOrderByIdAsync

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }


        #endregion

        #region GetOrderForSuccessAsync

        public async Task<Order?> GetOrderForSuccessAsync(int orderId)
        {
            return await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }


        #endregion

    }
}