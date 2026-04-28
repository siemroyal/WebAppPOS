using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppPOS.Data;
using WebAppPOS.Models;

namespace WebAppPOS.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            // Fetch orders with customer info, ordered by newest first
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .OrderByDescending(o => o.OrderDate)
                .AsNoTracking()
                .ToListAsync();

            return View(orders);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                    .ThenInclude(p => p.Product) // Fetch product name for line items
                .FirstOrDefaultAsync(m => m.OrderId == id);

            if (order == null) return NotFound();

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewBag.Customers = _context.Customers.ToList();
            ViewBag.Products = _context.Products.Where(p => p.StockQty > 0).ToList();

            // Generate Invoice Number (e.g., INV-00001)
            var lastOrder = _context.Orders.OrderByDescending(o => o.OrderId).FirstOrDefault();
            int nextId = (lastOrder?.OrderId ?? 0) + 1;
            ViewBag.InvoiceNo = "INV-" + nextId.ToString("D5");

            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            if (!ModelState.IsValid)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    // 1. Save the main Order
                    _context.Add(order);
                    await _context.SaveChangesAsync();

                    // 2. Process Order Details and Reduce Stock
                    if (order.OrderDetails != null)
                    {
                        foreach (var detail in order.OrderDetails)
                        {
                            // Logic: reduceStockProduct()
                            var product = await _context.Products.FindAsync(detail.ProductId);
                            if (product != null)
                            {
                                if (product.StockQty < detail.Quantity)
                                {
                                    throw new Exception($"Insufficient stock for {product.ProductName}");
                                }
                                product.StockQty -= detail.Quantity;
                                _context.Update(product);
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", "Error processing order: " + ex.Message);
                }
            }

            // If we reach here, something failed. Repopulate ViewBags.
            ViewBag.Customers = _context.Customers.ToList();
            ViewBag.Products = _context.Products.ToList();
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(m => m.OrderId == id);

            if (order == null) return NotFound();

            ViewBag.Customers = _context.Customers.ToList();
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(m => m.OrderId == id);

            if (order != null)
            {
                // Optional: Logic to RESTORE stock if order is canceled/deleted
                foreach (var detail in order.OrderDetails)
                {
                    var product = await _context.Products.FindAsync(detail.ProductId);
                    if (product != null) product.StockQty += detail.Quantity;
                }

                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
