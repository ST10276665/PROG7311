using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROG7311.Data;
using PROG7311.Models;
using PROG7311.Patterns.Observers;
using PROG7311.Patterns.Strategies;

namespace PROG7311.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ServiceRequestValidator _validator;
        private readonly ContractNotifier _notifier;
        private readonly ManagerNotifier _managerNotifier;

        public ServiceRequestsController(
            ApplicationDbContext context,
            ServiceRequestValidator validator,
            ContractNotifier notifier,
            ManagerNotifier managerNotifier)
        {
            _context = context;
            _validator = validator;
            _notifier = notifier;
            _managerNotifier = managerNotifier;
            _notifier.Subscribe(_managerNotifier);
        }

        public async Task<IActionResult> Index()
        {
            var requests = await _context.ServiceRequests
                .Include(s => s.Contract)
                .ThenInclude(c => c.Client)
                .ToListAsync();
            return View(requests);
        }

        public async Task<IActionResult> Details(int id)
        {
            var request = await _context.ServiceRequests
                .Include(s => s.Contract)
                .ThenInclude(c => c.Client)
                .FirstOrDefaultAsync(s => s.ServiceRequestId == id);

            if (request == null) return NotFound();
            return View(request);
        }

        public IActionResult Create()
        {
            ViewBag.Contracts = new SelectList(
                _context.Contracts.Include(c => c.Client),
                "ContractId",
                "ServiceLevel");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceRequest request)
        {
            ModelState.Remove("Contract");

            var contract = await _context.Contracts.FindAsync(request.ContractId);

            if (contract == null)
            {
                ModelState.AddModelError("ContractId", "Selected contract not found.");
            }
            else
            {
                var (isValid, errorMessage) = _validator.Validate(request, contract);
                if (!isValid)
                {
                    ModelState.AddModelError(string.Empty, errorMessage);
                }
            }

            if (ModelState.IsValid)
            {
                _context.ServiceRequests.Add(request);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Contracts = new SelectList(
                _context.Contracts.Include(c => c.Client),
                "ContractId",
                "ServiceLevel",
                request.ContractId);
            return View(request);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var request = await _context.ServiceRequests.FindAsync(id);
            if (request == null) return NotFound();

            ViewBag.Contracts = new SelectList(
                _context.Contracts.Include(c => c.Client),
                "ContractId",
                "ServiceLevel",
                request.ContractId);
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServiceRequest request)
        {
            if (id != request.ServiceRequestId) return NotFound();

            ModelState.Remove("Contract");

            var contract = await _context.Contracts.FindAsync(request.ContractId);

            if (contract == null)
            {
                ModelState.AddModelError("ContractId", "Selected contract not found.");
            }
            else
            {
                var (isValid, errorMessage) = _validator.Validate(request, contract);
                if (!isValid)
                {
                    ModelState.AddModelError(string.Empty, errorMessage);
                }
            }

            if (ModelState.IsValid)
            {
                _context.Update(request);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Contracts = new SelectList(
                _context.Contracts.Include(c => c.Client),
                "ContractId",
                "ServiceLevel",
                request.ContractId);
            return View(request);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var request = await _context.ServiceRequests
                .Include(s => s.Contract)
                .FirstOrDefaultAsync(s => s.ServiceRequestId == id);

            if (request == null) return NotFound();
            return View(request);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var request = await _context.ServiceRequests.FindAsync(id);
            if (request != null)
            {
                _context.ServiceRequests.Remove(request);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}