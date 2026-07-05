using ArtHouse.Data;
using ArtHouse.Models;
using ArtHouse.ViewModels;
using ArtHouse.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ArtHouse.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        #region Our Private Methods

        #region LoadCategories
        private void LoadCategories()
        {
            ViewBag.Categories = _context.Categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
        }
        #endregion

        #region SaveImage

        private string SaveImage(IFormFile image)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
                fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                image.CopyTo(stream);
            }

            return "/images/" + fileName;
        }

        #endregion

        #region DeleteImage

        private void DeleteImage(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return;

            var imagePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                imageUrl.TrimStart('/')
            );

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        #endregion

        #endregion



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

        //********************

        public IActionResult Create()
        {

            var model = new ProductCreateViewModel();

            LoadCategories(model);

            return View(model);
        }

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
                product.ImageUrl = SaveImage(model.Image);
            }

            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        //********************


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
                DeleteImage(product.ImageUrl);

                product.ImageUrl = SaveImage(model.Image);
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }




        //********************

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

    }
}

