using ArtHouse.Data;
using Microsoft.AspNetCore.Mvc;
using ArtHouse.Models;

namespace ArtHouse.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var category = _context.Categories.ToList();
            return View(category);
        }
    }
}

