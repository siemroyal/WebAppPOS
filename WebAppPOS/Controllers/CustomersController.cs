using Microsoft.AspNetCore.Mvc;

namespace WebAppPOS.Controllers
{
    using global::WebAppPOS.Data;
    using global::WebAppPOS.Models;
    using global::WebAppPOS.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    namespace WebAppPOS.Controllers
    {
        public class CustomersController : Controller
        {
            private readonly ApplicationDbContext _context;
            private readonly ImageService _imageService;

            public CustomersController(ApplicationDbContext context, ImageService imageService)
            {
                _context = context;
                _imageService = imageService;
            }

            // GET: Customers
            public async Task<IActionResult> Index()
            {
                var customers = await _context.Customers
                    .AsNoTracking()
                    .ToListAsync();
                return View(customers);
            }

            // GET: Customers/Details/5
            public async Task<IActionResult> Details(int id)
            {
                var customer = await _context.Customers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.CustomerId == id);

                if (customer == null) return NotFound();

                return View(customer);
            }

            // GET: Customers/Create
            [HttpGet]
            public IActionResult Create()
            {
                return View();
            }

            // POST: Customers/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(Customer customer)
            {
                if (customer.CustomerImage != null)
                {
                    // Saving image to uploads/customers
                    customer.PhotoUrl = await _imageService
                        .SaveImageAsync(customer.CustomerImage, "uploads/customers");
                }

                if (ModelState.IsValid)
                {
                    _context.Customers.Add(customer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                return View(customer);
            }

            // GET: Customers/Edit/5
            [HttpGet]
            public async Task<IActionResult> Edit(int id)
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer == null) return NotFound();

                return View(customer);
            }

            // POST: Customers/Edit/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(Customer customer)
            {
                var existingCustomer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);

                if (existingCustomer == null) return NotFound();

                if (customer.CustomerImage != null)
                {
                    // Delete old image if it exists
                    if (!string.IsNullOrEmpty(existingCustomer.PhotoUrl))
                    {
                        _imageService.DeleteImageAsync(existingCustomer.PhotoUrl);
                    }

                    // Save new image
                    existingCustomer.PhotoUrl = await _imageService
                        .SaveImageAsync(customer.CustomerImage, "uploads/customers");
                }

                if (ModelState.IsValid)
                {
                    // Mapping properties manually to existing tracked entity
                    existingCustomer.CustomerName = customer.CustomerName;
                    existingCustomer.Email = customer.Email;
                    existingCustomer.Phone = customer.Phone;
                    existingCustomer.Address = customer.Address;
                    existingCustomer.Type = customer.Type;
                    existingCustomer.BankName = customer.BankName;
                    existingCustomer.AccountHolder = customer.AccountHolder;
                    existingCustomer.AccountNumber = customer.AccountNumber;

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                return View(customer);
            }

            // POST: Customers/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var customer = await _context.Customers.FindAsync(id);

                if (customer == null) return NotFound();

                // Delete image file from server
                if (!string.IsNullOrEmpty(customer.PhotoUrl))
                {
                    _imageService.DeleteImageAsync(customer.PhotoUrl);
                }

                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
        }
    }
}
