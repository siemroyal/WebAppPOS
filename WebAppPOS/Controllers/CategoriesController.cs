using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppPOS.Data;
using WebAppPOS.Models;
using WebAppPOS.Services;

namespace WebAppPOS.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context; //
        private readonly IWebHostEnvironment _env;
        private readonly ImageService _imageService;     
        //Property
        //Action method
        //Constructor
        public CategoriesController(ApplicationDbContext context, IWebHostEnvironment env,ImageService imageService)
        {
            this._context = context;
            _env = env;
            _imageService = imageService;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category, IFormFile ImageUrl)
        {
            if (string.IsNullOrEmpty(category.CategoryName))
            {
                ModelState.AddModelError("CategoryName", "Category name is required!");
            }
            if (string.IsNullOrEmpty(category.Description))
            {
                ModelState.AddModelError("Description", "Description is required");
            }
            if (ModelState.IsValid)
            {
                return View(category);
            }
            if(category.FileImage != null)
            {
                var fileName = await _imageService.SaveImageAsync(category.FileImage, "categories");
                category.ImageUrl = fileName;
            }
           
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
            return View(category);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category,IFormFile ImageUrl)
        {
            if (!string.IsNullOrEmpty(category.ImageUrl))
            {
                var path = Path.Combine(_env.WebRootPath, category.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            if (category.ImageUrl != null)
            {
                string photoPath = null;
                if (ImageUrl != null && ImageUrl.Length > 0)
                {
                    var uploads = Path.Combine(_env.WebRootPath, "images");
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }
                    var extension = Path.GetExtension(ImageUrl.FileName);
                    var fileName = Guid.NewGuid() + extension;
                    var filePath = Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageUrl.CopyToAsync(stream);
                    }
                    photoPath = "/images/" + fileName;
                }
                category.ImageUrl = photoPath;
            }
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if(category != null)
            {
                if (!string.IsNullOrEmpty(category.ImageUrl))
                {
                    var path = Path.Combine(_env.WebRootPath, category.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
