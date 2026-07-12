using ArtHouse.Data;
using ArtHouse.Models;
using ArtHouse.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;

namespace ArtHouse.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }


        #region Read

        public IActionResult Index()
        {
            var model = new CategoryListViewModel();

            model.Categories = _context.Categories
                .Select(c => new CategoryItemViewModel
                {
                    Name = c.Name,
                    Id = c.Id
                })
                .ToList();

            return View(model);
        }

        #endregion

        //*************************

        #region Create

        //[HttpGet]
        //public IActionResult Create()
        //{
        //    var model = new CategoryCreateViewModel();

        //    model.Name

        //    return View(model);
        //}


        #endregion


    }
}

