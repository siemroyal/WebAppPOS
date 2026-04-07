using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppPOS.Data;
using WebAppPOS.Models;
using WebAppPOS.Models.Enums;
using WebAppPOS.Services;

namespace WebAppPOS.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly ApplicationDbContext _context; 
        public PurchaseController(ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<IActionResult> Index(string search)
        {
            var purchasesQuery = _context.Purchases
                .Include(p => p.Suppliers)
                .Include(p => p.PurchaseDetails)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                purchasesQuery = purchasesQuery.Where(p =>
                    p.PurchaseNo.Contains(search) ||
                    p.Suppliers.SupplierName.Contains(search));
            }

            var list = await purchasesQuery.OrderByDescending(p => p.PurchaseDate).ToListAsync();
            return View(list);
        }

        private void PopulateViewBag()
        {
            ViewBag.Suppliers = _context.Suppliers.ToList();
            ViewBag.Products = _context.Products.Select(p => new {
                p.Id,
                p.ProductName,
                p.Cost
            }).ToList();
        }

        public IActionResult Details(int id)
        {
            var purchase = _context.Purchases
                .Include(p => p.Suppliers)
                .Include(p => p.PurchaseDetails)
                .ThenInclude(d => d.Products)
                .FirstOrDefault(p => p.PurchaseId == id);

            if (purchase == null) return NotFound();

            return View(purchase);
        }

        public IActionResult Create() {
            var count = _context.Purchases.Count() + 1;
            ViewBag.PurchaseNo = "PUR-" + count.ToString("D4");

            PopulateViewBag();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Purchase model)
        {
            if (!ModelState.IsValid)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var purchase = new Purchase
                    {
                        PurchaseNo = model.PurchaseNo,
                        PurchaseDate = model.PurchaseDate,
                        SupplierId = model.SupplierId,
                        PurchaseStatus = model.PurchaseStatus,
                        CreatedBy = 1
                    };

                    _context.Purchases.Add(purchase);
                    await _context.SaveChangesAsync();

                    foreach (var item in model.PurchaseDetails)
                    {
                        var detail = new PurchaseDetail
                        {
                            PurchaseId = purchase.PurchaseId,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice
                        };
                        _context.PurchaseDetails.Add(detail);
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                }
            }
            PopulateViewBag();
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var purchase = _context.Purchases
                .Include(p => p.PurchaseDetails)
                .FirstOrDefault(p => p.PurchaseId == id);

            if (purchase == null) return NotFound();

            ViewBag.Suppliers = _context.Suppliers.ToList();
            ViewBag.Products = _context.Products.ToList();

            return View(purchase);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Purchase purchase)
        {
            if (id != purchase.PurchaseId) return NotFound();

            if (!ModelState.IsValid)
            {
                var existing = _context.Purchases
                    .Include(p => p.PurchaseDetails)
                    .FirstOrDefault(p => p.PurchaseId == id);

                if (existing == null) return NotFound();

                // Update header
                existing.PurchaseDate = purchase.PurchaseDate;
                existing.SupplierId = purchase.SupplierId;
                existing.PurchaseStatus = purchase.PurchaseStatus;

                // Remove old details
                _context.PurchaseDetails.RemoveRange(existing.PurchaseDetails);

                // Add new details
                existing.PurchaseDetails = purchase.PurchaseDetails
                    .Where(x => x.ProductId != 0 && x.PurchaseId > 0)
                    .ToList();

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Suppliers = _context.Suppliers.ToList();
            ViewBag.Products = _context.Products.ToList();

            return View(purchase);
        }
    }
}
