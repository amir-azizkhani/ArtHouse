using ArtHouse.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtHouse.Identity;

namespace ArtHouse.ViewComponents
{
    public class CartBadgeViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartBadgeViewComponent(
            AppDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return View(0);
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return View(0);
            }

            var cartItemCount = await _context.CartItems
                               .Where(ci => ci.Cart.UserId == user.Id)
                               .SumAsync(ci => ci.Quantity);

            return View(cartItemCount);
        }
    }
}