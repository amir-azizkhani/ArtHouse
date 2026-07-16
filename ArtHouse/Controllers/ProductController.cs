using ArtHouse.Data;
using ArtHouse.Models;
using ArtHouse.Services;
using ArtHouse.ViewModels;
using ArtHouse.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace ArtHouse.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ImageService _imageService;
        public ProductController(AppDbContext context, ImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        #region Read
        public IActionResult Index(string? search)
        {
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Title.Contains(search));
            }

            var products = query
                .Select(p => new ProductItemViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Price = p.Price,
                    Stock = p.Stock,
                    ImageUrl = p.ImageUrl,
                    CategoryName = p.Category != null ? p.Category.Name : ""
                })
                .ToList();

            var viewModel = new ProductListViewModel
            {
                Products = products,
                Search = search,
                TotalCount = products.Count
            };

            return View(viewModel);
        }
        #endregion

        //********************

        #region Create
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {

            var model = new ProductCreateViewModel();

            LoadCategories(model);

            return View(model);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                LoadCategories(model);
                return View(model);
            }

            var product = new Product
            {
                Title = model.Title,
                Price = model.Price,
                Stock = model.Stock,
                CategoryId = model.CategoryId
            };

            if (model.Image == null)
            {
                Console.WriteLine("Image is NULL");
            }
            else
            {
                Console.WriteLine(model.Image.FileName);
            }

            if (model.Image != null && model.Image.Length > 0)
            {
                product.ImageUrl = _imageService.SaveImage(model.Image);
            }

            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        #endregion

        //********************

        #region Edit
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var model = new ProductEditViewModel
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                ExistingImageUrl = product.ImageUrl
            };

            LoadCategories(model);

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                LoadCategories(model);

                return View(model);
            }

            var product = _context.Products.FirstOrDefault(p => p.Id == model.Id);

            if (product == null)
            {
                return NotFound();
            }

            product.Title = model.Title;
            product.Description = model.Description;
            product.Price = model.Price;
            product.Stock = model.Stock;
            product.CategoryId = model.CategoryId;

            if (model.Image != null && model.Image.Length > 0)
            {
                _imageService.DeleteImage(product.ImageUrl);

                product.ImageUrl = _imageService.SaveImage(model.Image);
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        #endregion

        //********************

        #region Delete
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Delete(int id)
        {

            var product = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var model = new ProductDeleteViewModel
            {
                Id = product.Id,
                Title = product.Title,
                Price = product.Price,
                ImageUrl = product.ImageUrl,

                CategoryName = product.Category?.Name ?? string.Empty
            };

            return View(model);

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(ProductDeleteViewModel model)
        {

            var product = _context.Products.FirstOrDefault(p => p.Id == model.Id);

            if (product == null)
            {
                return NotFound();
            }

            _imageService.DeleteImage(product.ImageUrl);

            _context.Products.Remove(product);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        #endregion

        //********************

        #region Our Private Methods

        #region LoadCategories
        private void LoadCategories(ProductCreateViewModel model)
        {
            model.Categories = _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();
        }
        #endregion

        #endregion

    }
}

