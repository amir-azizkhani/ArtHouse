using ArtHouse.Data;
using ArtHouse.Identity;
using ArtHouse.Models.Enums;
using ArtHouse.ViewModels.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtHouse.Services.Interfaces;

namespace ArtHouse.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderService _orderService;

        public OrderController(AppDbContext context, UserManager<ApplicationUser> userManager, IOrderService orderService)
        {
            _context = context;
            _userManager = userManager;
            _orderService = orderService;
        }

        #region Success
        public async Task<IActionResult> Success(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }


            var orders = await _orderService.GetUserOrdersAsync(user.Id);


            var orderViewModels = orders.Select(o => new OrderItemListViewModel
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalPrice = o.TotalPrice,
                TotalItems = o.OrderItems.Sum(oi => oi.Quantity)
            })
            .ToList();


            var model = new OrderListViewModel
            {
                Orders = orderViewModels
            };


            return View(model);
        }
        #endregion

        #region Details
        //Instead of creating 2 seprate actions for admin and users we made the action dynamic and practical for both the user and admin!

        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }


            var order = await _orderService.GetOrderByIdAsync(id);


            if (order == null)
            {
                return NotFound();
            }


            // Users can only see their own orders
            if (!User.IsInRole("Admin") && order.UserId != user.Id)
            {
                return NotFound();
            }


            var model = new OrderDetailsViewModel
            {
                OrderId = order.Id,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,

                Items = order.OrderItems.Select(oi => new OrderDetailsItemViewModel
                {
                    Title = oi.Product.Title,
                    ImageUrl = oi.Product.ImageUrl,
                    Price = oi.Price,
                    Quantity = oi.Quantity
                }).ToList()
            };


            return View(model);
        }

        #endregion

        #region AdminOrders

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();


            var orderViewModels = orders.Select(o => new OrderItemListViewModel
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalPrice = o.TotalPrice,
                TotalItems = o.OrderItems.Sum(oi => oi.Quantity),
                UserName = o.User.UserName!,
                Status = o.Status
            })
            .ToList();


            var model = new OrderListViewModel
            {
                Orders = orderViewModels
            };


            return View(model);
        }


        #endregion

        #region UpdateStatus

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int orderId, OrderStatus status)
        {
            var result = await _orderService.UpdateStatusAsync(orderId, status);

            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }


        #endregion

    }
}