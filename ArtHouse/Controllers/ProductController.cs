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

        #region

        private void LoadCategories()
        {
            ViewBag.Categories = _context.Categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
        }

        #endregion

        #endregion



            //public IActionResult Index()
            //{
            //    var products = _context.Products.Include(p => p.Category).Select(p => new ProductItemViewModel
            //                                                                    {
            //                                                                        Id = p.Id,
            //                                                                        Title = p.Title,
            //                                                                        Price = p.Price,
            //                                                                        Stock = p.Stock,
            //                                                                        ImageUrl = p.ImageUrl,
            //                                                                        CategoryName = p.Category.Name
            //                                                                    }).ToList();

            //    var viewModel = new ProductListViewModel
            //    {
            //        Products = products,
            //        TotalCount = products.Count
            //    };

            //    return View(viewModel);
            //}

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

            if (model.Image != null && model.Image.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString()
                    + Path.GetExtension(model.Image.FileName);

                var path = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/images",
                    fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    model.Image.CopyTo(stream);
                }

                product.ImageUrl = "/images/" + fileName;
            }

            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }





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

