using ArtHouse.Data;
using ArtHouse.Models;
using ArtHouse.Services;
using ArtHouse.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ArtHouse.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ImageService _imageService;
        public CategoryController(AppDbContext context, ImageService imageService)
        {
            _context = context;
            _imageService = imageService;
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
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new CategoryCreateViewModel();

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var category = new Category
            {
                Name = model.Name
            };

            if (model.Image != null)
            {
                category.ImageUrl = _imageService.SaveImage(model.Image);
            }

            _context.Categories.Add(category);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        #endregion

        //*************************

        #region Edit

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            var model = new CategoryEditViewModel
            {
                Id = category.Id,
                Name = category.Name,
                ExistingImageUrl = category.ImageUrl
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var category = _context.Categories.FirstOrDefault(c => c.Id == model.Id);

            if (category == null)
            {
                return NotFound();
            }

            category.Name = model.Name;

            if (model.Image != null && model.Image.Length > 0)
            {

                _imageService.DeleteImage(category.ImageUrl);

                category.ImageUrl = _imageService.SaveImage(model.Image);
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        #endregion

        //*************************

        #region Delete

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            var model = new CategoryDeleteViewModel
            {
                Id = category.Id,
                Name = category.Name,
                ImageUrl = category.ImageUrl
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(CategoryDeleteViewModel model)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == model.Id);

            if (category == null)
            {
                return NotFound();
            }

            _imageService.DeleteImage(category.ImageUrl);

            _context.Categories.Remove(category);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        #endregion







    }
}

