using ArtHouse.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtHouse.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }


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
    }
}