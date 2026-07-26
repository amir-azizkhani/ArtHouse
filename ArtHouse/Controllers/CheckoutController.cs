using ArtHouse.Identity;
using ArtHouse.Models;
using ArtHouse.Models.Enums;
using ArtHouse.ViewModels.Orders;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtHouse.Services.Interfaces;

namespace ArtHouse.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(UserManager<ApplicationUser> userManager, ICheckoutService checkoutService)
        {
            _userManager = userManager;
            _checkoutService = checkoutService;
        }

        #region Index

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }


            var model = await _checkoutService.GetCheckoutAsync(user.Id);


            if (model == null)
            {
                return RedirectToAction("Index", "Cart");
            }


            return View(model);
        }

        #endregion

        #region PlaceOrder

        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }


            var order = await _checkoutService.PlaceOrderAsync(user.Id);


            if (order == null)
            {
                return RedirectToAction("Index", "Cart");
            }


            return RedirectToAction("Success", "Order", new { id = order.Id });
        }

        #endregion


    }
}