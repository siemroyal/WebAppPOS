using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppPOS.Data;

namespace WebAppPOS.Controllers
{
    public class UnitController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UnitController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var units = await _context.Units
                .AsNoTracking()
                .ToListAsync();
            return View(units);
        }
    }
}
