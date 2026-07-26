using ArtHouse.Identity;
using ArtHouse.Models;
using ArtHouse.ViewModels.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtHouse.Services.Interfaces;

namespace ArtHouse.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICartService _cartService;

        public CartController(UserManager<ApplicationUser> userManager, ICartService cartService)
        {
            _userManager = userManager;
            _cartService = cartService;
        }


        #region AddToCart

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }


            await _cartService.AddToCartAsync(user.Id, productId);


            return RedirectToAction("Index", "Product");
        }

        #endregion

        #region IncreaseQuantity

        [HttpPost]
        public async Task<IActionResult> IncreaseQuantity(int cartItemId)
        {
            var result = await _cartService.IncreaseQuantityAsync(cartItemId);

            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region DecreaseQuantity

        [HttpPost]
        public async Task<IActionResult> DecreaseQuantity(int cartItemId)
        {
            var result = await _cartService.DecreaseQuantityAsync(cartItemId);


            if (!result)
            {
                return NotFound();
            }


            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Remove

        [HttpPost]
        public async Task<IActionResult> Remove(int cartItemId)
        {
            var result = await _cartService.RemoveAsync(cartItemId);


            if (!result)
            {
                return NotFound();
            }


            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Index

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);


            if (user == null)
            {
                return Challenge();
            }


            var model = await _cartService.GetCartAsync(user.Id);


            return View(model);
        }

        #endregion




    }
}