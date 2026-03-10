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
        private readonly ImageService _imageService;
        public ProductsController(ApplicationDbContext context, ImageService imageService)
        {
            _context = context;
            this._imageService = imageService;
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

        public async Task<IActionResult> Details(int id)
        {
            var products = await _context.Products
                .Include(c => c.Category)
                .Include(u => u.Unit)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
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
            if (product.ProudctImage != null)
            {
                product.ProductUrl = await _imageService
                    .SaveImageAsync(product.ProudctImage, "uploads/products");
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
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            DropdownCategoryAndUnit();
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
            if (existingProduct == null) return NotFound();
            if (product.ProudctImage != null)
            {
                if (existingProduct.ProductUrl != null)
                {
                    _imageService.DeleteImageAsync(existingProduct.ProductUrl);
                }
                existingProduct.ProductUrl = await _imageService
                    .SaveImageAsync(product.ProudctImage, "uploads/products");
            }
            if (ModelState.IsValid)
            {
                existingProduct.ProductCode = product.ProductCode;
                existingProduct.ProductName = product.ProductName;
                existingProduct.Price = product.Price;
                existingProduct.Cost = product.Cost;
                existingProduct.Unit = product.Unit;
                existingProduct.Category = product.Category;
                existingProduct.StockQty = product.StockQty;

                //_context.Products.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            DropdownCategoryAndUnit();
            return View(product);
           
        }
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)return NotFound();
            if (!string.IsNullOrEmpty(product.ProductUrl))
            {
                _imageService.DeleteImageAsync(product.ProductUrl);
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
