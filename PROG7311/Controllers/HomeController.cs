using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG7311.Data;

namespace PROG7311.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalClients = await _context.Clients.CountAsync();
            ViewBag.TotalContracts = await _context.Contracts.CountAsync();
            ViewBag.TotalServiceRequests = await _context.ServiceRequests.CountAsync();
            ViewBag.ActiveContracts = await _context.Contracts.CountAsync(c => c.Status == "Active");
            ViewBag.PendingRequests = await _context.ServiceRequests.CountAsync(r => r.Status == "Pending");
            ViewBag.RecentContracts = await _context.Contracts
                .Include(c => c.Client)
                .OrderByDescending(c => c.ContractId)
                .Take(5)
                .ToListAsync();
            return View();
        }
    }
}