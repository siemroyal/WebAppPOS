using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppPOS.Data;
using WebAppPOS.Models;
using WebAppPOS.Services;

namespace WebAppPOS.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ImageService imageService;
        public ProductsController(ApplicationDbContext context, ImageService imageService)
        {
            _context = context;
            this.imageService = imageService;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(c => c.Category)
                .Include(u => u.Unit)
                .AsNoTracking()
                .ToListAsync();
            return View(products);
        }
        private void DropdownCategoryAndUnit()
        {
            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewBag.UnitId = new SelectList(_context.Units, "UnitId", "UnitName");
        }
        [HttpGet]
        public IActionResult Create()
        {
            DropdownCategoryAndUnit();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if(product.ProudctImage != null)
            {
                product.ProductUrl = await imageService
                    .SaveImageAsync(product.ProudctImage,"uploads/products");
            }

            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            DropdownCategoryAndUnit();
            return View();
        }

    }
}
//Claude.AI / ClaudAI
