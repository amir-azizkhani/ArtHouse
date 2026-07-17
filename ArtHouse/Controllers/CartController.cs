using ArtHouse.Data;
using ArtHouse.Identity;
using ArtHouse.Models;
using ArtHouse.ViewModels.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtHouse.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(
            AppDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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

            var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == user.Id);


            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = user.Id
                };

                _context.Carts.Add(cart);

                await _context.SaveChangesAsync();
            }


            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);



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

            return RedirectToAction("Index", "Product");
        }


        #endregion

        #region IncreaseQuantity

        [HttpPost]
        public async Task<IActionResult> IncreaseQuantity(int cartItemId)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId);

            if (cartItem == null)
            {
                return NotFound();
            }

            cartItem.Quantity++;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        #endregion

        #region DecreaseQuantity

        [HttpPost]
        public async Task<IActionResult> DecreaseQuantity(int cartItemId)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId);

            if (cartItem == null)
            {
                return NotFound();
            }

            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region

        [HttpPost]
        public async Task<IActionResult> Remove(int cartItemId)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId);

            if (cartItem == null)
            {
                return NotFound();
            }

            _context.CartItems.Remove(cartItem);

            await _context.SaveChangesAsync();

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

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            var viewModel = new CartViewModel();

            if (cart == null)
            {
                return View(viewModel);
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

            return View(viewModel);
        }


        #endregion




    }
}