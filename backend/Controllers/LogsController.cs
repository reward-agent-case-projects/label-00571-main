using Dqsm.Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dqsm.Backend.Controllers
{
    public class LogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Show latest 100 logs
            var logs = await _context.Logs
                .OrderByDescending(l => l.DateTime)
                .Take(100)
                .ToListAsync();
            return View(logs);
        }
    }
}
