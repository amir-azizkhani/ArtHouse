using ArtHouse.Data;
using ArtHouse.Identity;
using ArtHouse.Models.Enums;
using ArtHouse.ViewModels.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtHouse.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        #region Success
        public async Task<IActionResult> Success(int id)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id);


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

            var orders = await _context.Orders
                .Where(o => o.UserId == user.Id)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new OrderItemListViewModel
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalPrice,
                    TotalItems = o.OrderItems.Sum(oi => oi.Quantity)
                })
                .ToListAsync();

            var model = new OrderListViewModel
            {
                Orders = orders
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

            var query = _context.Orders.AsQueryable();

            // We use this code that if a user in which they are not an admin they will see only their orders but if ths uder is our admin he can see all the users orders!
            if (!User.IsInRole("Admin"))
            {
                query = query.Where(o => o.UserId == user.Id);
            }

            var order = await query
                .Where(o => o.Id == id)
                .Select(o => new OrderDetailsViewModel
                {
                    OrderId = o.Id,
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalPrice,

                    Items = o.OrderItems.Select(oi => new OrderDetailsItemViewModel
                    {
                        Title = oi.Product.Title,
                        ImageUrl = oi.Product.ImageUrl,
                        Price = oi.Price,
                        Quantity = oi.Quantity
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }



        #endregion

        #region AdminOrders

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminOrders()
        {
            var orders = await _context.Orders
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new OrderItemListViewModel
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalPrice,
                    TotalItems = o.OrderItems.Sum(oi => oi.Quantity),
                    UserName = o.User.UserName!,
                    Status = o.Status
                })
                .ToListAsync();

            var model = new OrderListViewModel
            {
                Orders = orders
            };

            return View(model);
        }


        #endregion

        #region UpdateStatus

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int orderId, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;

            await _context.SaveChangesAsync();

            return Ok();
        }


        #endregion

    }
}